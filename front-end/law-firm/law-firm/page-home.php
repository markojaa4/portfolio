<?php
/**
* Template Name: Home
*
*/
?>
<?php get_header(); ?>
<div class="main_content">
	<section class="top_slider">
		<div class="banner_background_container">
			<div class="main_container">
				<div class="top_slider_inner">
					<div class="top_slider_items">
						<?php $args = array( 'post_type' => 'top-slides', 'posts_per_page' => -1, 'post_status' => 'publish', 'orderby' => 'date', 'order' => 'ASC' );
						$loop = new WP_Query( $args );
						while ( $loop->have_posts() ) : $loop->the_post();
						echo '<div class="top_slider_item"><div class="top_slider_image" style="background: url(' . get_the_post_thumbnail_url() . ') no-repeat; background-size: cover;"></div></div>';
				  	endwhile;
						wp_reset_postdata(); ?>
					</div>
					<div class="pager"></div>
				</div>
			</div>
		</div>
	</section>
	<section class="welcome">
		<div class="main_container outer_padding_left clearfix">
			<article class="welcome_article">
				<h1 class="main_title"><?php the_title(); ?></h1>
				<?php while ( have_posts() ) : the_post();
        the_content();
        endwhile; ?>
			</article>
			<div class="location">
				<a class="location_marker" href="<?php echo bloginfo('wpurl') . '/'. 'contact-us/'; ?>">Our Office</a>
			</div>
		</div>
	</section>
	<?php
	$args = array( 'post_type' => 'page', 's' => 'Practice Areas', 'sentence' => true, 'exact' => true, 'post_parent' => '0', 'posts_per_page' => 1, 'post_status' => 'publish' );
	$loop = new WP_Query( $args );
	while ( $loop->have_posts() ) : $loop->the_post();
	$practice_areas_id = get_the_ID();
  endwhile;
	$args = array( 'post_type' => 'page', 'post_parent' => $practice_areas_id, 'posts_per_page' => -1, 'post_status' => 'publish', 'orderby' => 'date', 'order' => 'ASC' );
	$loop = new WP_Query( $args );
	if ( $loop->have_posts() ) :
	echo '<section class="services"><div class="main_container"><h2 class="main_title outer_padding_left">Our Services</h2><div class="service_slider"><button class="slider_arrow arrow_left" type="button" aria-label="Slide back"></button><div class="service_slider_items_wrapper"><div class="service_slider_items">';
	while ( $loop->have_posts() ) : $loop->the_post();
	echo '<div class="service_slider_item"><div class="service_slider_item_inner"><h3>';
	the_title();
	echo '</h3><div class="service_slider_image" style="background: url(' . get_field('slider_thumbnail') . ') center no-repeat;"></div>';
	echo '<p class="service_slider_paragraph">' . get_post_meta(get_the_ID(), 'slider_item', true) . '</p>';
	echo '<a class="black_button" href=' . get_permalink() . '>Read More</a></div></div>';
	endwhile;
	echo '</div></div><button class="slider_arrow arrow_right" type="button" aria-label="Slide forward"></button></div></div></section>';
	wp_reset_postdata();
  endif; ?>
</div>
<?php get_footer(); ?>
