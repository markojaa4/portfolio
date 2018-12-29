<?php
/**
 * The template for displaying pages
 *
 * This is the template that displays all pages by default.
 * Please note that this is the WordPress construct of pages and that
 * other "pages" on your WordPress site will use a different template.
 *
 * @package WordPress
 * @subpackage MRK_Advocate
 * @since MRK Advocates
 */
 ?>
 <?php get_header(); ?>
 <div class="main_content pages_container">
 	<section class="banner">
 		<div class="banner_background_container">
 			<div class="main_container">
        <?php echo '<div class="banner_image" style="background: url(' . get_the_post_thumbnail_url() . ') no-repeat; background-size: cover;"></div>'; ?>
 			</div>
 		</div>
 	</section>
 	<section class="main_container outer_padding_both page_content">
 		<article>
 			<h1 class="page_title"><?php the_title(); ?></h1>
 			<?php while ( have_posts() ) : the_post();
 			the_content();
 			endwhile; ?>
 		</article>
 	</section>
 </div>
 <?php get_footer(); ?>
