<?php
/**
 * The template for displaying the footer
 *
 * Contains the closing of the #content div and all content after
 *
 * @package WordPress
 * @subpackage MRK_Advocates
 * @since MRK Advocates
 */
?>
			</main>
			<footer>
				<div class="main_container outer_padding_both">
					<section class="footer_navigation">
						<h3>General Navigation:</h3>
						<div>
							<?php wp_nav_menu( array('menu' => 'Footer General Navigation') ); ?>
						</div>
					</section>
					<section class="footer_practice_area">
						<h3>Pracice Area:</h3>
						<div>
							<?php wp_nav_menu( array('menu' => 'Footer Practice Area') ); ?>
						</div>
					</section>
					<section class="address_section">
						<?php $args = array( 'post_type' => 'footer-address', 'posts_per_page' => 1, 'post_status' => 'published' );
		      	$loop = new WP_Query( $args );
		        while ( $loop->have_posts() ) : $loop->the_post();
						echo '<h3>';
						the_title();
						echo '</h3>';
						the_content();
						wp_reset_postdata();
					  endwhile; ?>
					</section>
					<section class="footer_misc_section">
						<div class="privacy_policy">
							<a href="/privacy-policy/" title="Privacy Policy">Privacy Policy</a>
						</div>
						<div class="ipoint_link">
							<a href="http://ipoint.com.mt/" target="_blank" class="ftIpointLogo">
								<span class="ipointLogoSpriteAnimation"></span>
								<img src="<?php bloginfo('template_url'); ?>/images/ipoint-logo-sprite.png" alt="ipoint logo sprite" class="preloadSpriteAnimImage">
							</a>
						</div>
					</section>
				</div>
			</footer>
		</div>
		<?php wp_enqueue_script('jquery'); ?>
		<?php if(is_page_template("page-home.php")) { ?>
		<script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/hammer.js/2.0.8/hammer.min.js"></script>
		<?php } ?>
		<?php wp_footer(); ?>
		<script src="<?php bloginfo('template_url'); ?>/js/main.js<?php echo '?ver='.filemtime(dirname(__FILE__) . '/js/main.js'); ?>" type="text/javascript"></script>
		<?php if(is_page_template("page-home.php")) { ?>
		<script src="<?php bloginfo('template_url'); ?>/js/home-page.js<?php echo '?ver='.filemtime(dirname(__FILE__) . '/js/home-page.js'); ?>" type="text/javascript"></script>
	<?php } elseif(is_page_template("page-contact-us.php")) { ?>
		<script src="<?php bloginfo('template_url'); ?>/js/contact-us.js<?php echo '?ver='.filemtime(dirname(__FILE__) . '/js/contact-us.js'); ?>" type="text/javascript"></script>
		<script src="http://maps.googleapis.com/maps/api/js?key=&callback=initMap" async defer></script>
	<?php } elseif(is_page_template("page-practice-areas.php") OR is_page_template("page-the-firm.php") OR is_page_template("page-firm-members.php")) { ?>
		<script src="<?php bloginfo('template_url'); ?>/js/left-sidebar.js<?php echo '?ver='.filemtime(dirname(__FILE__) . '/js/left-sidebar.js'); ?>" type="text/javascript"></script>
	  <?php } ?>
	</body>
</html>
