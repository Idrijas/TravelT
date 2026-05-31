-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: May 31, 2026 at 04:26 AM
-- Server version: 10.4.32-MariaDB
-- PHP Version: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `travelt`
--

-- --------------------------------------------------------

--
-- Table structure for table `achievement`
--

CREATE TABLE `achievement` (
  `achievement_id` int(11) NOT NULL,
  `title` varchar(100) NOT NULL,
  `description` text NOT NULL,
  `icon_url` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `category`
--

CREATE TABLE `category` (
  `trip_id` int(11) NOT NULL,
  `category_id` int(11) NOT NULL,
  `category` enum('adventure','culture','food','nature','city','beach','other') NOT NULL,
  `tag_name` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `country`
--

CREATE TABLE `country` (
  `country_id` int(11) NOT NULL,
  `country_name` varchar(100) NOT NULL,
  `country_code` varchar(10) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `country`
--

INSERT INTO `country` (`country_id`, `country_name`, `country_code`) VALUES
(1, 'Afghanistan', 'AF'),
(2, 'Albania', 'AL'),
(3, 'Algeria', 'DZ'),
(4, 'Andorra', 'AD'),
(5, 'Angola', 'AO'),
(6, 'Argentina', 'AR'),
(7, 'Armenia', 'AM'),
(8, 'Australia', 'AU'),
(9, 'Austria', 'AT'),
(10, 'Azerbaijan', 'AZ'),
(11, 'Bahamas', 'BS'),
(12, 'Bahrain', 'BH'),
(13, 'Bangladesh', 'BD'),
(14, 'Belarus', 'BY'),
(15, 'Belgium', 'BE'),
(16, 'Belize', 'BZ'),
(17, 'Benin', 'BJ'),
(18, 'Bhutan', 'BT'),
(19, 'Bolivia', 'BO'),
(20, 'Bosnia and Herzegovina', 'BA'),
(21, 'Botswana', 'BW'),
(22, 'Brazil', 'BR'),
(23, 'Brunei', 'BN'),
(24, 'Bulgaria', 'BG'),
(25, 'Burkina Faso', 'BF'),
(26, 'Cambodia', 'KH'),
(27, 'Cameroon', 'CM'),
(28, 'Canada', 'CA'),
(29, 'Chile', 'CL'),
(30, 'China', 'CN'),
(31, 'Colombia', 'CO'),
(32, 'Costa Rica', 'CR'),
(33, 'Croatia', 'HR'),
(34, 'Cuba', 'CU'),
(35, 'Cyprus', 'CY'),
(36, 'Czech Republic', 'CZ'),
(37, 'Denmark', 'DK'),
(38, 'Dominican Republic', 'DO'),
(39, 'Ecuador', 'EC'),
(40, 'Egypt', 'EG'),
(41, 'El Salvador', 'SV'),
(42, 'Estonia', 'EE'),
(43, 'Ethiopia', 'ET'),
(44, 'Finland', 'FI'),
(45, 'France', 'FR'),
(46, 'Georgia', 'GE'),
(47, 'Germany', 'DE'),
(48, 'Ghana', 'GH'),
(49, 'Greece', 'GR'),
(50, 'Guatemala', 'GT'),
(51, 'Haiti', 'HT'),
(52, 'Honduras', 'HN'),
(53, 'Hungary', 'HU'),
(54, 'Iceland', 'IS'),
(55, 'India', 'IN'),
(56, 'Indonesia', 'ID'),
(57, 'Iran', 'IR'),
(58, 'Iraq', 'IQ'),
(59, 'Ireland', 'IE'),
(60, 'Israel', 'IL'),
(61, 'Italy', 'IT'),
(62, 'Jamaica', 'JM'),
(63, 'Japan', 'JP'),
(64, 'Jordan', 'JO'),
(65, 'Kazakhstan', 'KZ'),
(66, 'Kenya', 'KE'),
(67, 'Kuwait', 'KW'),
(68, 'Kyrgyzstan', 'KG'),
(69, 'Laos', 'LA'),
(70, 'Latvia', 'LV'),
(71, 'Lebanon', 'LB'),
(72, 'Libya', 'LY'),
(73, 'Liechtenstein', 'LI'),
(74, 'Lithuania', 'LT'),
(75, 'Luxembourg', 'LU'),
(76, 'Madagascar', 'MG'),
(77, 'Malaysia', 'MY'),
(78, 'Maldives', 'MV'),
(79, 'Mali', 'ML'),
(80, 'Malta', 'MT'),
(81, 'Mexico', 'MX'),
(82, 'Moldova', 'MD'),
(83, 'Monaco', 'MC'),
(84, 'Mongolia', 'MN'),
(85, 'Montenegro', 'ME'),
(86, 'Morocco', 'MA'),
(87, 'Mozambique', 'MZ'),
(88, 'Myanmar', 'MM'),
(89, 'Namibia', 'NA'),
(90, 'Nepal', 'NP'),
(91, 'Netherlands', 'NL'),
(92, 'New Zealand', 'NZ'),
(93, 'Nicaragua', 'NI'),
(94, 'Nigeria', 'NG'),
(95, 'North Korea', 'KP'),
(96, 'North Macedonia', 'MK'),
(97, 'Norway', 'NO'),
(98, 'Oman', 'OM'),
(99, 'Pakistan', 'PK'),
(100, 'Panama', 'PA'),
(101, 'Paraguay', 'PY'),
(102, 'Peru', 'PE'),
(103, 'Philippines', 'PH'),
(104, 'Poland', 'PL'),
(105, 'Portugal', 'PT'),
(106, 'Qatar', 'QA'),
(107, 'Romania', 'RO'),
(108, 'Russia', 'RU'),
(109, 'Saudi Arabia', 'SA'),
(110, 'Serbia', 'RS'),
(111, 'Singapore', 'SG'),
(112, 'Slovakia', 'SK'),
(113, 'Slovenia', 'SI'),
(114, 'South Africa', 'ZA'),
(115, 'South Korea', 'KR'),
(116, 'Spain', 'ES'),
(117, 'Sri Lanka', 'LK'),
(118, 'Sweden', 'SE'),
(119, 'Switzerland', 'CH'),
(120, 'Syria', 'SY'),
(121, 'Taiwan', 'TW'),
(122, 'Thailand', 'TH'),
(123, 'Tunisia', 'TN'),
(124, 'Turkey', 'TR'),
(125, 'Ukraine', 'UA'),
(126, 'United Arab Emirates', 'AE'),
(127, 'United Kingdom', 'GB'),
(128, 'United States', 'US'),
(129, 'Uruguay', 'UY'),
(130, 'Uzbekistan', 'UZ'),
(131, 'Venezuela', 'VE'),
(132, 'Vietnam', 'VN'),
(133, 'Yemen', 'YE'),
(134, 'Zambia', 'ZM'),
(135, 'Zimbabwe', 'ZW');

-- --------------------------------------------------------

--
-- Table structure for table `posts`
--

CREATE TABLE `posts` (
  `post_id` int(11) NOT NULL,
  `user_id` int(11) NOT NULL,
  `trip_id` int(11) DEFAULT NULL,
  `description` varchar(255) NOT NULL,
  `timestamp` datetime NOT NULL,
  `imagepath` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `posts`
--

INSERT INTO `posts` (`post_id`, `user_id`, `trip_id`, `description`, `timestamp`, `imagepath`) VALUES
(1, 1, NULL, 'At the beach with my brothers having a great time!', '2026-04-28 15:17:14', 'Images/beachpost_petergriffin.png'),
(4, 1, NULL, 'While i was in italy i passed by pisa so i for sure had to visit this world wonder. The italians never fail to amaze us.', '2026-04-30 00:21:37', 'Images/pisapost_petergriffin.png'),
(6, 1, NULL, 'Went to santa monica pier with the fam, i love it and im gay', '2026-05-11 23:58:53', 'Images/peterfamilyvacation.jpg'),
(7, 1, NULL, 'a little selfie of myself, i looked great i had to', '2026-05-12 00:21:15', 'Images/peter_profilepic.jpg'),
(8, 2, NULL, 'my dog can make me really mad sometimes', '2026-05-15 01:33:10', 'Images/shaggy_invincipost.png');

-- --------------------------------------------------------

--
-- Table structure for table `post_comments`
--

CREATE TABLE `post_comments` (
  `comment_id` int(11) NOT NULL,
  `post_id` int(11) NOT NULL,
  `user_id` int(11) NOT NULL,
  `comment_text` text NOT NULL,
  `created_at` datetime NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `post_comments`
--

INSERT INTO `post_comments` (`comment_id`, `post_id`, `user_id`, `comment_text`, `created_at`) VALUES
(4, 1, 1, 'it was a nice day man', '2026-05-03 03:19:31'),
(5, 1, 1, 'hello', '2026-05-03 03:26:20'),
(6, 1, 1, 'i want to test something', '2026-05-03 03:26:32'),
(7, 1, 1, 'youre an absolute *****', '2026-05-03 03:26:41'),
(8, 1, 1, 'no sorry i did not mean that', '2026-05-03 03:26:50'),
(9, 1, 1, 'test', '2026-05-03 03:34:02'),
(10, 4, 1, 'typical dad photo haha', '2026-05-03 16:06:19'),
(11, 1, 1, 'hellooo', '2026-05-03 17:13:47'),
(13, 8, 1, 'chill out man ', '2026-05-15 01:57:44');

-- --------------------------------------------------------

--
-- Table structure for table `post_likes`
--

CREATE TABLE `post_likes` (
  `user_id` int(11) NOT NULL,
  `post_id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `post_likes`
--

INSERT INTO `post_likes` (`user_id`, `post_id`) VALUES
(1, 1),
(1, 4),
(1, 6),
(1, 7),
(1, 8);

-- --------------------------------------------------------

--
-- Table structure for table `rank`
--

CREATE TABLE `rank` (
  `rank_id` int(11) NOT NULL,
  `name` varchar(100) NOT NULL,
  `description` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `report`
--

CREATE TABLE `report` (
  `report_id` int(11) NOT NULL,
  `reason` varchar(255) NOT NULL,
  `description` text NOT NULL,
  `report_date` datetime NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `report_participant`
--

CREATE TABLE `report_participant` (
  `report_id` int(11) NOT NULL,
  `reporter_id` int(11) NOT NULL,
  `reported_user_id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `trip`
--

CREATE TABLE `trip` (
  `trip_id` int(11) NOT NULL,
  `date_from` date DEFAULT NULL,
  `date_to` date DEFAULT NULL,
  `is_flexible_date` tinyint(1) NOT NULL DEFAULT 0,
  `flexible_months` varchar(255) DEFAULT NULL,
  `max_people` int(11) NOT NULL,
  `trip_type` enum('roadtrip','hikes','vacation','city_trip','backpacking','camping','business','other') NOT NULL,
  `description` text NOT NULL,
  `is_public` tinyint(1) NOT NULL,
  `status` enum('completed','started') NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `trip`
--

INSERT INTO `trip` (`trip_id`, `date_from`, `date_to`, `is_flexible_date`, `flexible_months`, `max_people`, `trip_type`, `description`, `is_public`, `status`) VALUES
(2, NULL, NULL, 1, 'November 2026', 4, 'city_trip', 'We\'re gonna visit this stupid city', 1, 'started'),
(3, '2026-06-17', '2026-06-26', 0, NULL, 4, 'vacation', 'We just wanna hang out and adventure', 1, 'started');

-- --------------------------------------------------------

--
-- Table structure for table `trip_country`
--

CREATE TABLE `trip_country` (
  `trip_id` int(11) NOT NULL,
  `country_id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `trip_country`
--

INSERT INTO `trip_country` (`trip_id`, `country_id`) VALUES
(2, 2),
(3, 3);

-- --------------------------------------------------------

--
-- Table structure for table `trip_place`
--

CREATE TABLE `trip_place` (
  `trip_id` int(11) NOT NULL,
  `place_name` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `trip_place`
--

INSERT INTO `trip_place` (`trip_id`, `place_name`) VALUES
(2, 'Copenhagen'),
(3, 'Himare'),
(3, 'Vlore');

-- --------------------------------------------------------

--
-- Table structure for table `user`
--

CREATE TABLE `user` (
  `user_id` int(11) NOT NULL,
  `username` varchar(50) NOT NULL,
  `email` varchar(100) NOT NULL,
  `password_hash` varchar(255) NOT NULL,
  `first_name` varchar(50) NOT NULL,
  `last_name` varchar(50) NOT NULL,
  `date_of_birth` date NOT NULL,
  `gender` enum('male','female','other') NOT NULL,
  `bio` text NOT NULL,
  `profile_picture` varchar(255) NOT NULL,
  `role` varchar(20) NOT NULL DEFAULT 'user'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `user`
--

INSERT INTO `user` (`user_id`, `username`, `email`, `password_hash`, `first_name`, `last_name`, `date_of_birth`, `gender`, `bio`, `profile_picture`, `role`) VALUES
(1, 'fatpeterrealistic', 'fat@gmail.com', 'peter', 'peter', 'grifin', '2009-05-03', 'male', 'helooo madafakers', 'Images\\peter_profilepic.jpg', 'user'),
(2, 'shaggyboy', 'shaggy@gmail.com', 'fatter', 'shaggy', 'scooby', '2001-05-09', 'male', 'wassup boys a big travel guru here', 'Images/shaggy_pfp.jpg', 'user');

INSERT INTO `user` (`user_id`, `username`, `email`, `password_hash`, `first_name`, `last_name`, `date_of_birth`, `gender`, `bio`,`profile_picture`,`role`) VALUES
(3,'admin1', 'admin1@travelt.com', '1234', 'Admin', 'Ondrej', '2000-01-01','male', 'admin wassup',"Images/ProfilePictures/profilepic1.png" ,'admin');

-- --------------------------------------------------------

--
-- Table structure for table `user_achievement`
--

CREATE TABLE `user_achievement` (
  `user_id` int(11) NOT NULL,
  `achievement_id` int(11) NOT NULL,
  `date_earned` datetime NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `user_rank`
--

CREATE TABLE `user_rank` (
  `user_id` int(11) NOT NULL,
  `rank_id` int(11) NOT NULL,
  `assigned_at` datetime NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;


-- --------------------------------------------------------

--
-- Table structure for table `user_trip`
--

CREATE TABLE `user_trip` (
  `trip_id` int(11) NOT NULL,
  `user_id` int(11) NOT NULL,
  `role` enum('admin','member') NOT NULL DEFAULT 'member'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `user_trip`
--

INSERT INTO `user_trip` (`trip_id`, `user_id`, `role`) VALUES
(2, 1, 'admin'),
(3, 1, 'admin');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `achievement`
--
ALTER TABLE `achievement`
  ADD PRIMARY KEY (`achievement_id`);

--
-- Indexes for table `category`
--
ALTER TABLE `category`
  ADD PRIMARY KEY (`category_id`),
  ADD KEY `fk_category_trip` (`trip_id`);

--
-- Indexes for table `country`
--
ALTER TABLE `country`
  ADD PRIMARY KEY (`country_id`);




--
-- Indexes for table `posts`
--
ALTER TABLE `posts`
  ADD PRIMARY KEY (`post_id`),
  ADD KEY `posts_ibfk_1` (`user_id`),
  ADD KEY `posts_ibfk_2` (`trip_id`);

--
-- Indexes for table `post_comments`
--
ALTER TABLE `post_comments`
  ADD PRIMARY KEY (`comment_id`),
  ADD KEY `fk_comments_post` (`post_id`),
  ADD KEY `fk_comments_user` (`user_id`);

--
-- Indexes for table `post_likes`
--
ALTER TABLE `post_likes`
  ADD PRIMARY KEY (`user_id`,`post_id`),
  ADD KEY `fk_likes_post` (`post_id`);

--
-- Indexes for table `rank`
--
ALTER TABLE `rank`
  ADD PRIMARY KEY (`rank_id`);

--
-- Indexes for table `report`
--
ALTER TABLE `report`
  ADD PRIMARY KEY (`report_id`);

--
-- Indexes for table `report_participant`
--
ALTER TABLE `report_participant`
  ADD PRIMARY KEY (`report_id`),
  ADD KEY `fk_report_participant_reporter` (`reporter_id`),
  ADD KEY `fk_report_participant_reported` (`reported_user_id`);

--
-- Indexes for table `trip`
--
ALTER TABLE `trip`
  ADD PRIMARY KEY (`trip_id`);

--
-- Indexes for table `trip_country`
--
ALTER TABLE `trip_country`
  ADD PRIMARY KEY (`trip_id`,`country_id`),
  ADD KEY `fk_trip_country_trip` (`trip_id`),
  ADD KEY `fk_trip_country_country` (`country_id`);

--
-- Indexes for table `trip_place`
--
ALTER TABLE `trip_place`
  ADD PRIMARY KEY (`trip_id`,`place_name`);

--
-- Indexes for table `user`
--
ALTER TABLE `user`
  ADD PRIMARY KEY (`user_id`),
  ADD UNIQUE KEY `username` (`username`),
  ADD UNIQUE KEY `email` (`email`);


ALTER TABLE `user`
	ADD COLUMN `nationality_country_id` INT NULL;


ALTER TABLE `user`
	ADD CONSTRAINT `fk_user_nationality_country`
	FOREIGN KEY (`nationality_country_id`)
	REFERENCES `country`(`country_id`);

--
-- Indexes for table `user_achievement`
--
ALTER TABLE `user_achievement`
  ADD PRIMARY KEY (`user_id`,`achievement_id`),
  ADD KEY `fk_user_achievement_user` (`user_id`),
  ADD KEY `fk_user_achievement_achievement` (`achievement_id`);

--
-- Indexes for table `user_rank`
--
ALTER TABLE `user_rank`
  ADD PRIMARY KEY (`user_id`,`rank_id`),
  ADD KEY `fk_user_rank_user` (`user_id`),
  ADD KEY `fk_user_rank_rank` (`rank_id`);

--
-- Indexes for table `user_trip`
--
ALTER TABLE `user_trip`
  ADD PRIMARY KEY (`trip_id`,`user_id`),
  ADD KEY `fk_user_trip_trip` (`trip_id`),
  ADD KEY `fk_user_trip_user` (`user_id`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `achievement`
--
ALTER TABLE `achievement`
  MODIFY `achievement_id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `category`
--
ALTER TABLE `category`
  MODIFY `category_id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `country`
--
ALTER TABLE `country`
  MODIFY `country_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=136;

--
-- AUTO_INCREMENT for table `posts`
--
ALTER TABLE `posts`
  MODIFY `post_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;

--
-- AUTO_INCREMENT for table `post_comments`
--
ALTER TABLE `post_comments`
  MODIFY `comment_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=14;

--
-- AUTO_INCREMENT for table `rank`
--
ALTER TABLE `rank`
  MODIFY `rank_id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `report`
--
ALTER TABLE `report`
  MODIFY `report_id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `trip`
--
ALTER TABLE `trip`
  MODIFY `trip_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT for table `user`
--
ALTER TABLE `user`
  MODIFY `user_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `category`
--
ALTER TABLE `category`
  ADD CONSTRAINT `fk_category_trip` FOREIGN KEY (`trip_id`) REFERENCES `trip` (`trip_id`) ON DELETE CASCADE;

--
-- Constraints for table `posts`
--
ALTER TABLE `posts`
  ADD CONSTRAINT `posts_ibfk_1` FOREIGN KEY (`user_id`) REFERENCES `user` (`user_id`),
  ADD CONSTRAINT `posts_ibfk_2` FOREIGN KEY (`trip_id`) REFERENCES `trip` (`trip_id`);

--
-- Constraints for table `post_comments`
--
ALTER TABLE `post_comments`
  ADD CONSTRAINT `fk_comments_post` FOREIGN KEY (`post_id`) REFERENCES `posts` (`post_id`) ON DELETE CASCADE,
  ADD CONSTRAINT `fk_comments_user` FOREIGN KEY (`user_id`) REFERENCES `user` (`user_id`) ON DELETE CASCADE;

--
-- Constraints for table `post_likes`
--
ALTER TABLE `post_likes`
  ADD CONSTRAINT `fk_likes_post` FOREIGN KEY (`post_id`) REFERENCES `posts` (`post_id`) ON DELETE CASCADE,
  ADD CONSTRAINT `fk_likes_user` FOREIGN KEY (`user_id`) REFERENCES `user` (`user_id`) ON DELETE CASCADE;

--
-- Constraints for table `report_participant`
--
ALTER TABLE `report_participant`
  ADD CONSTRAINT `fk_report_participant_report` FOREIGN KEY (`report_id`) REFERENCES `report` (`report_id`) ON DELETE CASCADE,
  ADD CONSTRAINT `fk_report_participant_reported` FOREIGN KEY (`reported_user_id`) REFERENCES `user` (`user_id`) ON DELETE CASCADE,
  ADD CONSTRAINT `fk_report_participant_reporter` FOREIGN KEY (`reporter_id`) REFERENCES `user` (`user_id`) ON DELETE CASCADE;

--
-- Constraints for table `trip_country`
--
ALTER TABLE `trip_country`
  ADD CONSTRAINT `fk_trip_country_country` FOREIGN KEY (`country_id`) REFERENCES `country` (`country_id`) ON DELETE CASCADE,
  ADD CONSTRAINT `fk_trip_country_trip` FOREIGN KEY (`trip_id`) REFERENCES `trip` (`trip_id`) ON DELETE CASCADE;

--
-- Constraints for table `trip_place`
--
ALTER TABLE `trip_place`
  ADD CONSTRAINT `fk_trip_place_trip` FOREIGN KEY (`trip_id`) REFERENCES `trip` (`trip_id`) ON DELETE CASCADE;

--
-- Constraints for table `user_achievement`
--
ALTER TABLE `user_achievement`
  ADD CONSTRAINT `fk_user_achievement_achievement` FOREIGN KEY (`achievement_id`) REFERENCES `achievement` (`achievement_id`) ON DELETE CASCADE,
  ADD CONSTRAINT `fk_user_achievement_user` FOREIGN KEY (`user_id`) REFERENCES `user` (`user_id`) ON DELETE CASCADE;

--
-- Constraints for table `user_rank`
--
ALTER TABLE `user_rank`
  ADD CONSTRAINT `fk_user_rank_rank` FOREIGN KEY (`rank_id`) REFERENCES `rank` (`rank_id`) ON DELETE CASCADE,
  ADD CONSTRAINT `fk_user_rank_user` FOREIGN KEY (`user_id`) REFERENCES `user` (`user_id`) ON DELETE CASCADE;

--
-- Constraints for table `user_trip`
--
ALTER TABLE `user_trip`
  ADD CONSTRAINT `fk_user_trip_trip` FOREIGN KEY (`trip_id`) REFERENCES `trip` (`trip_id`) ON DELETE CASCADE,
  ADD CONSTRAINT `fk_user_trip_user` FOREIGN KEY (`user_id`) REFERENCES `user` (`user_id`) ON DELETE CASCADE;

INSERT INTO achievement (achievement_id, title, description, icon_url) VALUES
(1, 'Create a Profile', 'Create your TravelT profile.', 'Images/Achievements/create_a_profile_icon.png'),
(2, 'Add Bio', 'Add a biography to your profile.', 'Images/Achievements/add_bio_icon.png'),
(3, 'Add Profile Picture', 'Upload your first profile picture.', 'Images/Achievements/add_profile_pic_icon.png'),
(4, 'Add First Post', 'Create your first post.', 'Images/Achievements/add_first_post_icon.png'),
(5, 'Create First Trip', 'Create your first trip.', 'Images/Achievements/create_first_trip_icon.png'),
(6, 'Create Five Trips', 'Create five trips.', 'Images/Achievements/create_five_trips_icon.png');


INSERT INTO rank (rank_id, name, description)
VALUES
(1, 'Wanderer', 'Create account, add biography/profile picture and complete first trip'),
(2, 'Scout', 'Complete 5 trips and visit 2 countries'),
(3, 'Explorer', 'Complete 10 trips and visit 5 countries'),
(4, 'Voyager', 'Complete 25 trips, visit 10 countries and add 5 travel posts'),
(5, 'Globetrotter', 'Complete 50 trips, visit 20 countries and add 10 travel posts'),
(6, 'Phileas Fogg', 'Complete 80 trips, visit 40 countries and add 20 travel posts');


CREATE TABLE IF NOT EXISTS `user_visited_country` (
    `user_id` int(11) NOT NULL,
    `country_id` int(11) NOT NULL,
    `visited_at` datetime DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;




COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
