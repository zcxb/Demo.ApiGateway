-- MySQL dump 10.13  Distrib 5.5.62, for Win64 (AMD64)
--
-- Host: 127.0.0.1    Database: test
-- ------------------------------------------------------
-- Server version	8.0.21

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `GlobalConfiguration`
--

DROP TABLE IF EXISTS `GlobalConfiguration`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `GlobalConfiguration` (
  `GlobalId` int NOT NULL AUTO_INCREMENT,
  `GatewayName` varchar(100) COLLATE utf8mb4_general_ci NOT NULL,
  `RequestIdKey` varchar(100) COLLATE utf8mb4_general_ci DEFAULT NULL,
  `BaseUrl` varchar(100) COLLATE utf8mb4_general_ci DEFAULT NULL,
  `DownstreamScheme` varchar(50) COLLATE utf8mb4_general_ci DEFAULT NULL,
  `ServiceDiscoveryProvider` varchar(500) COLLATE utf8mb4_general_ci DEFAULT NULL,
  `LoadBalancerOptions` varchar(500) COLLATE utf8mb4_general_ci DEFAULT NULL,
  `HttpHandlerOptions` varchar(500) COLLATE utf8mb4_general_ci DEFAULT NULL,
  `QoSOptions` varchar(200) COLLATE utf8mb4_general_ci DEFAULT NULL,
  `IsDefault` int NOT NULL,
  `InfoStatus` int NOT NULL,
  PRIMARY KEY (`GlobalId`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `GlobalConfiguration`
--

LOCK TABLES `GlobalConfiguration` WRITE;
/*!40000 ALTER TABLE `GlobalConfiguration` DISABLE KEYS */;
INSERT INTO `GlobalConfiguration` VALUES (1,'测试网关','test_gateway',NULL,NULL,NULL,NULL,NULL,NULL,1,1);
/*!40000 ALTER TABLE `GlobalConfiguration` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Route`
--

DROP TABLE IF EXISTS `Route`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `Route` (
  `RouteId` int NOT NULL AUTO_INCREMENT,
  `ItemId` int DEFAULT NULL,
  `UpstreamPathTemplate` varchar(150) COLLATE utf8mb4_general_ci NOT NULL,
  `UpstreamHttpMethod` varchar(50) COLLATE utf8mb4_general_ci NOT NULL,
  `UpstreamHost` varchar(100) COLLATE utf8mb4_general_ci DEFAULT NULL,
  `DownstreamScheme` varchar(50) COLLATE utf8mb4_general_ci NOT NULL,
  `DownstreamPathTemplate` varchar(200) COLLATE utf8mb4_general_ci NOT NULL,
  `DownstreamHostAndPorts` varchar(500) COLLATE utf8mb4_general_ci DEFAULT NULL,
  `AuthenticationOptions` varchar(300) COLLATE utf8mb4_general_ci DEFAULT NULL,
  `RequestIdKey` varchar(100) COLLATE utf8mb4_general_ci DEFAULT NULL,
  `CacheOptions` varchar(200) COLLATE utf8mb4_general_ci DEFAULT NULL,
  `ServiceName` varchar(100) COLLATE utf8mb4_general_ci DEFAULT NULL,
  `LoadBalancerOptions` varchar(500) COLLATE utf8mb4_general_ci DEFAULT NULL,
  `QoSOptions` varchar(200) COLLATE utf8mb4_general_ci DEFAULT NULL,
  `DelegatingHandlers` varchar(200) COLLATE utf8mb4_general_ci DEFAULT NULL,
  `Priority` int DEFAULT NULL,
  `InfoStatus` int NOT NULL,
  PRIMARY KEY (`RouteId`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Route`
--

LOCK TABLES `Route` WRITE;
/*!40000 ALTER TABLE `Route` DISABLE KEYS */;
INSERT INTO `Route` VALUES (1,NULL,'/api1/values','[ \"GET\" ]',NULL,'http','/api/Values','[{\"Host\": \"localhost\",\"Port\": 9000 }]',NULL,NULL,NULL,NULL,NULL,NULL,NULL,0,1);
/*!40000 ALTER TABLE `Route` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `RouteItem`
--

DROP TABLE IF EXISTS `RouteItem`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `RouteItem` (
  `ItemId` int NOT NULL AUTO_INCREMENT,
  `ItemName` varchar(100) COLLATE utf8mb4_general_ci NOT NULL,
  `ItemDetail` varchar(500) COLLATE utf8mb4_general_ci DEFAULT NULL,
  `ItemParentId` int DEFAULT NULL,
  `InfoStatus` int NOT NULL,
  PRIMARY KEY (`ItemId`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `RouteItem`
--

LOCK TABLES `RouteItem` WRITE;
/*!40000 ALTER TABLE `RouteItem` DISABLE KEYS */;
INSERT INTO `RouteItem` VALUES (1,'test',NULL,NULL,1);
/*!40000 ALTER TABLE `RouteItem` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `RouteMapping`
--

DROP TABLE IF EXISTS `RouteMapping`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `RouteMapping` (
  `MappingId` int NOT NULL AUTO_INCREMENT,
  `GlobalId` int DEFAULT NULL,
  `RouteId` int DEFAULT NULL,
  PRIMARY KEY (`MappingId`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `RouteMapping`
--

LOCK TABLES `RouteMapping` WRITE;
/*!40000 ALTER TABLE `RouteMapping` DISABLE KEYS */;
INSERT INTO `RouteMapping` VALUES (1,1,1);
/*!40000 ALTER TABLE `RouteMapping` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping routines for database 'test'
--
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2020-08-21 14:41:41
