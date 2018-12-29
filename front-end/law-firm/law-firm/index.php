<?php
/**
 * The main template file
 *
 * This is the most generic template file in a WordPress theme
 * and one of the two required files for a theme (the other being style.css).
 * It is used to display a page when nothing more specific matches a query.
 * E.g., it puts together the home page when no home.php file exists.
 *
 * @link http://codex.wordpress.org/Template_Hierarchy
 *
 * @package WordPress
 * @subpackage MRK_Advocates
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
