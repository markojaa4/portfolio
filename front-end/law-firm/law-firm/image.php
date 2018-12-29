<?php
/**
 * The template for displaying image attachments
 *
 * @package WordPress
 * @subpackage Twenty_Sixteen
 * @since Twenty Sixteen 1.0
 */

get_header(); ?>
	<div id="primary" class="content-area">
		<main id="main" class="site-main" role="main">
			<?php
				while ( have_posts() ) : the_post();
			?>
				<article id="post-<?php the_ID(); ?>" <?php post_class(); ?>>
					<header class="entry-header">
						<?php the_title( '<h1 class="entry-title">', '</h1>' ); ?>
					</header>
					<div class="entry-content">
						<div class="entry-attachment">
							<?php echo wp_get_attachment_image( get_the_ID(), 'thumbnail' );?>
						</div>
						<?php the_content(); ?>
					</div>
				</article>
				<?php
				endwhile;
			?>
		</main>
	</div>
<?php get_footer(); ?>
