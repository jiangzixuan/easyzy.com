CREATE TABLE `t_zy` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `UserId` int(11) NOT NULL,
  `BodyWordPath` varchar(30) NOT NULL,
  `BodyHtmlPath` varchar(30) NOT NULL,
  `AnswerWordPath` varchar(30) NOT NULL,
  `AnswerHtmlPath` varchar(30) NOT NULL,
  `CreateDate` datetime NOT NULL,
  `Ip` varchar(15) NOT NULL,
  `IMEI` varchar(15) NOT NULL,
  `MobileBrand` varchar(10) NOT NULL,
  `SystemType` varchar(10) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Id_UNIQUE` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=1000001 DEFAULT CHARSET=utf8;

alter table easyzy_home.t_zy auto_increment=1000000;