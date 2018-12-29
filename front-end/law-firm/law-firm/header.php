<?php
/**
 * The template for displaying the header
 *
 * Displays all of the head element and everything up until the "site-content" div.
 *
 * @package WordPress
 * @subpackage MRK_Advocates
 * @since MRK Advocates
 */

?><!DOCTYPE html>
<html <?php language_attributes(); ?> class="no-js">
	<head>
		<meta charset="<?php bloginfo( 'charset' ); ?>">
		<meta name="viewport" content="width=device-width, initial-scale=1.0, minimum-scale=1.0, maximum-scale=1.0, user-scalable=no">
		<title><?php wp_title( '|', true, 'right' );bloginfo('name'); ?></title>
		<link rel="profile" href="http://gmpg.org/xfn/11">
		<link rel="shortcut icon" type="image/x-icon" href="<?php bloginfo('template_url');?>/images/favicon.ico" />
		<?php if ( is_singular() && pings_open( get_queried_object() ) ) : ?>
		<link rel="pingback" href="<?php bloginfo( 'pingback_url' ); ?>">
		<?php endif; ?>
		<link rel="stylesheet" href="<?php bloginfo('stylesheet_url'); echo '?ver=' . filemtime( get_stylesheet_directory() . '/style.css'); ?>" type="text/css" media="all" />
		<?php wp_head(); ?>
	</head>
	<body <?php body_class(); ?>>
		<div class="page_wrapper">
	    <div class="mobile_menu_overlay"></div>
	    <div class="mobile_menu">
	      <nav>
	        <?php wp_nav_menu( array('menu' => 'Mobile Header Menu') ); ?>
	      </nav>
	    </div>
	    <header>
	      <div class="main_container outer_padding_both clearfix">
	        <a class="main_logo" href="<?php bloginfo('wpurl'); ?>">
	          <img src="<?php bloginfo('template_url'); ?>/images/logo.png" alt="M&amp;RK Advocates logo">
	        </a>
	        <div class="header_menu">
	          <nav>
	            <?php wp_nav_menu( array('menu' => 'Header Menu') ); ?>
	          </nav>
	          <div id="top_menu_hamburger_button" class="hamburger_button"><span></span><span></span><span></span></div>
	        </div>
	      </div>
	    </header>
	    <main>
