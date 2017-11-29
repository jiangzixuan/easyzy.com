CREATE TABLE `t_user` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `UserName` varchar(20) NOT NULL,
  `TrueName` varchar(5) NOT NULL,
  `Psd` varchar(32) NOT NULL,
  `Mobile` varchar(11) NOT NULL,
  `FirstLoginDate` datetime NOT NULL,
  `CreateDate` datetime NOT NULL,
  `Extend1` varchar(20) NOT NULL COMMENT '扩展1，存放明文密码',
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Id_UNIQUE` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=1000000 DEFAULT CHARSET=utf8;

alter table easyzy_home.t_user auto_increment=1000000;