CREATE TABLE `t_zy` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `UserId` int(11) NOT NULL,
  `BodyWordPath` varchar(50) NOT NULL,
  `BodyHtmlPath` varchar(50) NOT NULL,
  `AnswerWordPath` varchar(50) NOT NULL,
  `AnswerHtmlPath` varchar(50) NOT NULL,
  `CreateDate` datetime NOT NULL,
  `Ip` varchar(15) NOT NULL,
  `IMEI` varchar(15) NOT NULL,
  `MobileBrand` varchar(10) NOT NULL,
  `SystemType` varchar(10) NOT NULL,
  `Browser` varchar(15) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Id_UNIQUE` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=1000001 DEFAULT CHARSET=utf8;

alter table easyzy_home.t_zy auto_increment=1000000;

CREATE TABLE `t_zystruct` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `ZyId` int(11) NOT NULL,
  `BqNum` int(11) NOT NULL COMMENT '�����',
  `SqNum` int(11) NOT NULL COMMENT 'С���',
  `QuesType` int(11) NOT NULL COMMENT '0���͹��⣻1��������',
  `QuesAnswer` varchar(4) NOT NULL COMMENT '�͹���𰸣�������桮��',
  `CreateDate` datetime NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Id_UNIQUE` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;