using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hw.easyzy.model.entity
{
    public class T_KnowledgePoints
    {
        /// <summary>
		/// Id
		/// </summary>
        public int Id { get; set; }

        /// <summary>
        /// CourseId
        /// </summary>
        public int CourseId { get; set; }

        /// <summary>
        /// Depth
        /// </summary>
        public int Depth { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Ordinal
        /// </summary>
        public int Ordinal { get; set; }

        /// <summary>
        /// ParentId
        /// </summary>
        public int ParentId { get; set; }

        /// <summary>
        /// NODE\KNOWLEDGE_POINT\TESTING_POINT
        /// </summary>
        public string Type { get; set; }
    }
}
