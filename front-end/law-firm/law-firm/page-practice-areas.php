<?php
/**
 *
 * Template Name: Practice Areas
 *
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
	<section class="main_container outer_padding_both clearfix">
		<section class="left_sidebar">
			<button class="sidebar_toggle_button"></button>
			<div class="left_sidebar_inner">
				<nav>
					<?php wp_nav_menu( array('menu' => 'Practice Areas Sidebar') ); ?>
				</nav>
			</div>
		</section>
		<section class="page_content content_right">
			<article>
				<h1 class="page_title"><?php the_title(); ?></h1>
				<?php while ( have_posts() ) : the_post();
        the_content();
				if (get_field('footnote')) {
					echo '<p class="footnote">';
					the_field('footnote');
					echo '</p>';
				}
        endwhile; ?>
			</article>
		</section>
	</section>
</div>
<?php get_footer(); ?>
