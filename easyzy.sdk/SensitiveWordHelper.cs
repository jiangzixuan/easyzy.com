using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easyzy.sdk
{
    /// <summary>
    /// 敏感词帮助类
    /// </summary>
    public class SensitiveWordHelper
    {
        private HashSet<string> hash = new HashSet<string>();
        private byte[] fastCheck = new byte[char.MaxValue];
        private BitArray charCheck = new BitArray(char.MaxValue);
        private int maxWordLength = 0;
        private int minWordLength = int.MaxValue;
        private bool _isHave = false;
        private string _replaceString = "*";
        private char _splitString = '|';
        private string _newWord;
        private string _badWordFilePath;

        /// <summary>
        /// 是否含有脏字
        /// </summary>
        public bool IsHave
        {
            get { return _isHave; }
        }

        /// <summary>
        /// 替换后字符串
        /// </summary>
        public string ReplaceString
        {
            set { _replaceString = value; }
        }
        /// <summary>
        /// 脏字字典切割符
        /// </summary>
        public char SplitString
        {
            set { _splitString = value; }
        }

        /// <summary>
        /// 更新后的字符串
        /// </summary>
        public string NewWord
        {
            get { return _newWord; }
        }

        /// <summary>
        /// 脏字字典文档路径
        /// </summary>
        public string BadWordFilePath
        {
            get { return _badWordFilePath; }
            set { _badWordFilePath = value; }
        }

        public SensitiveWordHelper(string filePath)
        {
            _badWordFilePath = filePath;
            List<string> badworList = new List<string>();
            if (File.Exists(_badWordFilePath))
            {
                StreamReader sr = new StreamReader(_badWordFilePath, Encoding.GetEncoding("gb2312"));
                string input;
                while ((input = sr.ReadLine()) != null)
                {
                    badworList.Add(input.Trim());
                }
                sr.Close();
                sr.Dispose();
            }
            string[] badwords = badworList.ToArray();
            foreach (string word in badwords)
            {
                maxWordLength = Math.Max(maxWordLength, word.Length);
                minWordLength = Math.Min(minWordLength, word.Length);
                for (int i = 0; i < 7 && i < word.Length; i++)
                {
                    fastCheck[word[i]] |= (byte)(1 << i);
                }

                for (int i = 7; i < word.Length; i++)
                {
                    fastCheck[word[i]] |= 0x80;
                }

                if (word.Length == 1)
                {
                    charCheck[word[0]] = true;
                }
                else
                {
                    hash.Add(word);
                }
            }
        }
        public bool HasBadWord(string text)
        {
            int index = 0;

            while (index < text.Length)
            {

                if ((fastCheck[text[index]] & 1) == 0)
                {
                    while (index < text.Length - 1 && (fastCheck[text[++index]] & 1) == 0) ;
                }

                //单字节检测
                if (minWordLength == 1 && charCheck[text[index]])
                {
                    return true;
                }

                //多字节检测
                for (int j = 1; j <= Math.Min(maxWordLength, text.Length - index - 1); j++)
                {
                    //快速排除
                    if ((fastCheck[text[index + j]] & (1 << Math.Min(j, 7))) == 0)
                    {
                        break;
                    }

                    if (j + 1 >= minWordLength)
                    {
                        string sub = text.Substring(index, j + 1);

                        if (hash.Contains(sub))
                        {
                            return true;
                        }
                    }
                }
                index++;
            }
            return false;
        }

        public string ReplaceBadWord(string text)
        {
            int index = 0;

            for (index = 0; index < text.Length; index++)
            {
                if ((fastCheck[text[index]] & 1) == 0)
                {
                    while (index < text.Length - 1 && (fastCheck[text[++index]] & 1) == 0) ;
                }

                //单字节检测
                if (minWordLength == 1 && charCheck[text[index]])
                {
                    //return true;
                    _isHave = true;
                    text = text.Replace(text[index], _replaceString[0]);
                    continue;
                }
                //多字节检测
                for (int j = 1; j <= Math.Min(maxWordLength, text.Length - index - 1); j++)
                {

                    //快速排除
                    if ((fastCheck[text[index + j]] & (1 << Math.Min(j, 7))) == 0)
                    {
                        break;
                    }

                    if (j + 1 >= minWordLength)
                    {
                        string sub = text.Substring(index, j + 1);

                        if (hash.Contains(sub))
                        {

                            //替换字符操作
                            _isHave = true;
                            char cc = _replaceString[0];
                            string rp = _replaceString.PadRight((j + 1), cc);
                            text = text.Replace(sub, rp);
                            //记录新位置
                            index += j;
                            break;
                        }
                    }
                }
            }
            _newWord = text;
            return text;
        }
    }
}
