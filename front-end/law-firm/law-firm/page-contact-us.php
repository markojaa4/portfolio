<?php
/**
 *
 * Template Name: Contact Us
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
	<section class="main_container outer_padding_both page_content">
		<h1 class="page_title"><?php the_title(); ?></h1>
		<div class="contact_us_content clearfix">
			<div class="address_box">
				<?php while ( have_posts() ) : the_post();
				the_content();
			  endwhile; ?>
			</div>
			<form name="contact">
				<div class="form_input">
					<div class="form_fields">
						<input id="contacter_name" class="regular_field" name="full_name" type="text" placeholder="Full Name:">
						<input id="contacter_mail" class="email_field" name="email" type="text" placeholder="eMail:">
						<input id="contacter_subject" class="regular_field" name="subject" type="text" placeholder="Subject:">
						<textarea id="contacter_message" class="regular_field" name="message" placeholder="Message:"></textarea>
					</div>
					<div class="form_buttons">
						<input type="reset" value="Clear message" class="black_button">
						<input id="contact_submit" type="button" value="Send message" class="black_button float_right">
					</div>
				</div>
				<div class="form_result">
					<p></p>
				</div>
			</form>
			<div class="embed_map"></div>
		</div>
	</section>
</div>
<?php get_footer(); ?>
