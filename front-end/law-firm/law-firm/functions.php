<?php
/**
 * Twenty Sixteen functions and definitions
 *
 * Set up the theme and provides some helper functions, which are used in the
 * theme as custom template tags. Others are attached to action and filter
 * hooks in WordPress to change core functionality.
 *
 * When using a child theme you can override certain functions (those wrapped
 * in a function_exists() call) by defining them first in your child theme's
 * functions.php file. The child theme's functions.php file is included before
 * the parent theme's file, so the child theme functions would be used.
 *
 * @link https://codex.wordpress.org/Theme_Development
 * @link https://codex.wordpress.org/Child_Themes
 *
 * Functions that are not pluggable (not wrapped in function_exists()) are
 * instead attached to a filter or action hook.
 *
 * For more information on hooks, actions, and filters,
 * {@link https://codex.wordpress.org/Plugin_API}
 *
 * @package WordPress
 * @subpackage Twenty_Sixteen
 * @since Twenty Sixteen 1.0
 */

/**
 * Twenty Sixteen only works in WordPress 4.4 or later.
 */
if ( version_compare( $GLOBALS['wp_version'], '4.4-alpha', '<' ) ) {
	require get_template_directory() . '/inc/back-compat.php';
}

if ( ! function_exists( 'twentysixteen_setup' ) ) :
/**
 * Sets up theme defaults and registers support for various WordPress features.
 *
 * Note that this function is hooked into the after_setup_theme hook, which
 * runs before the init hook. The init hook is too late for some features, such
 * as indicating support for post thumbnails.
 *
 * Create your own twentysixteen_setup() function to override in a child theme.
 *
 * @since Twenty Sixteen 1.0
 */
function twentysixteen_setup() {
	//add_theme_support( 'automatic-feed-links' );
	add_theme_support( 'post-thumbnails' );
	set_post_thumbnail_size( 1200, 9999 );

	// This theme uses wp_nav_menu() in two locations.
	register_nav_menus( array(
		'primary' => __( 'Primary Menu', 'twentysixteen' ),
	) );

	/*
	 * Switch default core markup for search form, comment form, and comments
	 * to output valid HTML5.
	 */
	add_theme_support( 'html5', array(
		'search-form',
		'comment-form',
		'comment-list',
		'gallery',
		'caption',
	) );

	/*
	 * Enable support for Post Formats.
	 *
	 * See: https://codex.wordpress.org/Post_Formats
	 */
	add_theme_support( 'post-formats', array(
		'aside',
		'image',
		'video',
		'quote',
		'link',
		'gallery',
		'status',
		'audio',
		'chat',
	) );

	/*
	 * This theme styles the visual editor to resemble the theme style,
	 * specifically font, colors, icons, and column width.
	 */
	add_editor_style( array( 'css/editor-style.css', twentysixteen_fonts_url() ) );

	// Indicate widget sidebars can use selective refresh in the Customizer.
	add_theme_support( 'customize-selective-refresh-widgets' );
}
endif; // twentysixteen_setup
add_action( 'after_setup_theme', 'twentysixteen_setup' );

/**
 * Sets the content width in pixels, based on the theme's design and stylesheet.
 *
 * Priority 0 to make it available to lower priority callbacks.
 *
 * @global int $content_width
 *
 * @since Twenty Sixteen 1.0
 */
function twentysixteen_content_width() {
	$GLOBALS['content_width'] = apply_filters( 'twentysixteen_content_width', 840 );
}
add_action( 'after_setup_theme', 'twentysixteen_content_width', 0 );

/**
 * Registers a widget area.
 *
 * @link https://developer.wordpress.org/reference/functions/register_sidebar/
 *
 * @since Twenty Sixteen 1.0
 */
function twentysixteen_widgets_init() {
	register_sidebar( array(
		'name'          => __( 'Sidebar', 'twentysixteen' ),
		'id'            => 'sidebar-1',
		'description'   => __( 'Add widgets here to appear in your sidebar.', 'twentysixteen' ),
		'before_widget' => '<section id="%1$s" class="widget %2$s">',
		'after_widget'  => '</section>',
		'before_title'  => '<h2 class="widget-title">',
		'after_title'   => '</h2>',
	) );

	register_sidebar( array(
		'name'          => __( 'Content Bottom 1', 'twentysixteen' ),
		'id'            => 'sidebar-2',
		'description'   => __( 'Appears at the bottom of the content on posts and pages.', 'twentysixteen' ),
		'before_widget' => '<section id="%1$s" class="widget %2$s">',
		'after_widget'  => '</section>',
		'before_title'  => '<h2 class="widget-title">',
		'after_title'   => '</h2>',
	) );

	register_sidebar( array(
		'name'          => __( 'Content Bottom 2', 'twentysixteen' ),
		'id'            => 'sidebar-3',
		'description'   => __( 'Appears at the bottom of the content on posts and pages.', 'twentysixteen' ),
		'before_widget' => '<section id="%1$s" class="widget %2$s">',
		'after_widget'  => '</section>',
		'before_title'  => '<h2 class="widget-title">',
		'after_title'   => '</h2>',
	) );
}
add_action( 'widgets_init', 'twentysixteen_widgets_init' );

if ( ! function_exists( 'twentysixteen_fonts_url' ) ) :
/**
 * Register Google fonts for Twenty Sixteen.
 *
 * Create your own twentysixteen_fonts_url() function to override in a child theme.
 *
 * @since Twenty Sixteen 1.0
 *
 * @return string Google fonts URL for the theme.
 */
function twentysixteen_fonts_url() {
	$fonts_url = '';
	$fonts     = array();
	$subsets   = 'latin,latin-ext';

	/* translators: If there are characters in your language that are not supported by Merriweather, translate this to 'off'. Do not translate into your own language. */
	if ( 'off' !== _x( 'on', 'Merriweather font: on or off', 'twentysixteen' ) ) {
		$fonts[] = 'Merriweather:400,700,900,400italic,700italic,900italic';
	}

	/* translators: If there are characters in your language that are not supported by Montserrat, translate this to 'off'. Do not translate into your own language. */
	if ( 'off' !== _x( 'on', 'Montserrat font: on or off', 'twentysixteen' ) ) {
		$fonts[] = 'Montserrat:400,700';
	}

	/* translators: If there are characters in your language that are not supported by Inconsolata, translate this to 'off'. Do not translate into your own language. */
	if ( 'off' !== _x( 'on', 'Inconsolata font: on or off', 'twentysixteen' ) ) {
		$fonts[] = 'Inconsolata:400';
	}

	if ( $fonts ) {
		$fonts_url = add_query_arg( array(
			'family' => urlencode( implode( '|', $fonts ) ),
			'subset' => urlencode( $subsets ),
		), 'https://fonts.googleapis.com/css' );
	}

	return $fonts_url;
}
endif;

/**
 * Handles JavaScript detection.
 *
 * Adds a `js` class to the root `<html>` element when JavaScript is detected.
 *
 * @since Twenty Sixteen 1.0
 */
function twentysixteen_javascript_detection() {
	echo "<script>(function(html){html.className = html.className.replace(/\bno-js\b/,'js')})(document.documentElement);</script>\n";
}
add_action( 'wp_head', 'twentysixteen_javascript_detection', 0 );

/**
 * Enqueues scripts and styles.
 *
 * @since Twenty Sixteen 1.0
 */
function twentysixteen_scripts() {
	wp_localize_script( 'twentysixteen-script', 'screenReaderText', array(
		'expand'   => __( 'expand child menu', 'twentysixteen' ),
		'collapse' => __( 'collapse child menu', 'twentysixteen' ),
	) );
}
add_action( 'wp_enqueue_scripts', 'twentysixteen_scripts' );

/**
 * Adds custom classes to the array of body classes.
 *
 * @since Twenty Sixteen 1.0
 *
 * @param array $classes Classes for the body element.
 * @return array (Maybe) filtered body classes.
 */
function twentysixteen_body_classes( $classes ) {
	// Adds a class of custom-background-image to sites with a custom background image.
	if ( get_background_image() ) {
		$classes[] = 'custom-background-image';
	}

	// Adds a class of group-blog to sites with more than 1 published author.
	if ( is_multi_author() ) {
		$classes[] = 'group-blog';
	}

	// Adds a class of no-sidebar to sites without active sidebar.
	if ( ! is_active_sidebar( 'sidebar-1' ) ) {
		$classes[] = 'no-sidebar';
	}

	// Adds a class of hfeed to non-singular pages.
	if ( ! is_singular() ) {
		$classes[] = 'hfeed';
	}

	return $classes;
}
add_filter( 'body_class', 'twentysixteen_body_classes' );

/**
 * Converts a HEX value to RGB.
 *
 * @since Twenty Sixteen 1.0
 *
 * @param string $color The original color, in 3- or 6-digit hexadecimal form.
 * @return array Array containing RGB (red, green, and blue) values for the given
 *               HEX code, empty array otherwise.
 */
function twentysixteen_hex2rgb( $color ) {
	$color = trim( $color, '#' );

	if ( strlen( $color ) === 3 ) {
		$r = hexdec( substr( $color, 0, 1 ).substr( $color, 0, 1 ) );
		$g = hexdec( substr( $color, 1, 1 ).substr( $color, 1, 1 ) );
		$b = hexdec( substr( $color, 2, 1 ).substr( $color, 2, 1 ) );
	} else if ( strlen( $color ) === 6 ) {
		$r = hexdec( substr( $color, 0, 2 ) );
		$g = hexdec( substr( $color, 2, 2 ) );
		$b = hexdec( substr( $color, 4, 2 ) );
	} else {
		return array();
	}

	return array( 'red' => $r, 'green' => $g, 'blue' => $b );
}

/*require get_template_directory() . '/inc/template-tags.php';*/
require get_template_directory() . '/inc/customizer.php';

function meks_disable_srcset( $sources ) {
    return false;
}
add_filter( 'wp_calculate_image_srcset', 'meks_disable_srcset' );

function my_deregister_scripts(){
  wp_deregister_script( 'wp-embed' );
}
add_action( 'wp_footer', 'my_deregister_scripts' );


/*register_post_type('hp-banners', array(
    'labels' => array(
        'name' => __('Banners', 'stevetheclerk'),
        'singular_name' => __('Banners', 'stevetheclerk'),
        'menu_name' => __('Banners', 'stevetheclerk'),
        'add_new' => __('Add New Banner', 'stevetheclerk'),
        'add_new_item' => __('Add New Banner', 'stevetheclerk'),
        'edit_item' => __('Edit Banner', 'stevetheclerk'),
        'new_item' => __('New Banner', 'stevetheclerk'),
        'view_item' => __('View Banner', 'stevetheclerk'),
        'search_items' => __('Search Banners', 'stevetheclerk'),
        'not_found' => __('No Banners found', 'stevetheclerk'),
        'not_found_in_trash' => __('No Banners found in Trash', 'stevetheclerk'),
    ),
    'public' => TRUE,
    'rewrite' => array('slug' => 'hp-banners'),
    'has_archive' => false,
    'publicly_queryable' => FALSE,
    'supports' => array('title', 'thumbnail'),
));*/

/*function contact_form(){
    $userNameSurnameField = $_POST['userNameSurnameField'];
    $userEmailAddressField = $_POST['userEmailAddressField'];
    $userMessageText = $_POST['userMessageText'];

    $subject =  'Contact form';
    $adminSubject = 'New Contact Form Submission';
    $adminEmail = 'dev2@ipoint.com.mt';

    if (!empty($_SERVER['HTTP_CLIENT_IP'])) {
        $ip = $_SERVER['HTTP_CLIENT_IP'];
    } elseif (!empty($_SERVER['HTTP_X_FORWARDED_FOR'])) {
        $ip = $_SERVER['HTTP_X_FORWARDED_FOR'];
    } else {
        $ip = $_SERVER['REMOTE_ADDR'];
    }

  	$adminMessage = '
    <div style="background:white;padding:20px 0px 0px 0px;"><br/><br/>
        <div style="width:100%;display:block;font-family:Verdana;">Dear admin,</div><br/>
        <div style="width:100%;display:block;font-family:Verdana;">Kindly note that new contact form has been received. Find details below:</div><br/>
        <div style="width:100%;display:block;font-family:Verdana;line-height:20px;"><strong style="color:#006837;">Name & Surname:</strong> '.$userNameSurnameField.'</div>
        <div style="width:100%;display:block;font-family:Verdana;line-height:20px;"><strong style="color:#006837;">E-mail Address:</strong> '.$userEmailAddressField.'</div>
        <div style="width:100%;display:block;font-family:Verdana;line-height:20px;white-space: pre-wrap;"><strong style="color:#006837;">Message:</strong> '.$userMessageText.'</div>
        <br/>
        <br/>
        <a href="http://stevetheclerk.com/" target="_blank"><img src="'.get_template_directory_uri().'/images/mail-logo.png" alt="steve-the-clerk-logo"></a>
    </div>';
    $headers = 'From: Steve the Clerk <info@stevetheclerk.com>' . "\r\n";
    $headers .= "MIME-Version: 1.0" . "\r\n";
    $headers .= "Content-type:text/html;charset=utf-8" . "\r\n";
    mail($adminEmail, $adminSubject, $adminMessage, $headers);

    $userMessage = '
    <div style="background:white;padding:20px 0px 0px 0px;"><br/>
        <div style="width:100%;display:block;font-family:Verdana;">Dear '.$userNameSurnameField.',</div><br/>
      	<div style="width:100%;display:block;font-family:Verdana;">Thank you for contacting us. We will get back to you shortly.</div><br/><br/>
        <a href="http://stevetheclerk.com/" target="_blank"><img src="'.get_template_directory_uri().'/images/mail-logo.png" alt="steve-the-clerk-logo"></a><br/><br/>
    </div>';
    $header_1 = 'From: Steve the Clerk <info@stevetheclerk.com>' . "\r\n";
    $header_1 .= "MIME-Version: 1.0" . "\r\n";
    $header_1 .= "Content-type:text/html;charset=utf-8" . "\r\n";
    mail($userEmailAddressField, $subject, $userMessage, $header_1);

    global $wpdb;
    $wpdb->insert(
        'contact_forms',
        array(
            'name_surname' => $userNameSurnameField,
            'email_address' => $userEmailAddressField,
            'message' => $userMessageText,
            'ins_date' => current_time('Y-m-d, H:i:s'),
            'IP' => $ip
        ),
        array(
            '%s',
            '%s',
            '%s',
            '%s',
            '%s'
        )
    );

    die();
}
add_action('wp_ajax_contact_form', 'contact_form');
add_action('wp_ajax_nopriv_contact_form', 'contact_form');*/

add_action( 'init', 'remove_editor_init' );
function remove_editor_init() {
    if ( ! is_admin() ) {
       return;
    }

    $current_post_id = filter_input( INPUT_GET, 'post', FILTER_SANITIZE_NUMBER_INT );
    $update_post_id = filter_input( INPUT_POST, 'post_ID', FILTER_SANITIZE_NUMBER_INT );

    if ( isset( $current_post_id ) ) {
       $post_id = absint( $current_post_id );
    } else if ( isset( $update_post_id ) ) {
       $post_id = absint( $update_post_id );
    } else {
       return;
    }

    if ( isset( $post_id ) ) {
       $template_file = get_post_meta( $post_id, '_wp_page_template', true );

		/*if (  'page-home.php' === $template_file ) {
           remove_post_type_support( 'page', 'editor' );
		}*/
    }
}

function custom_login_logo() {
    echo '<style type="text/css">
    h1 a { background-image: url('.get_bloginfo('template_directory').'/images/logo.png) !important;height: 150px !important;width: 150px !important;background-size: 80% !important;outline: none !important;border: none !important;box-shadow: none !important;}
    body.login {background-color: #f8f3e6;}
    #login {padding: 5% 0 0 !important;}
    .login #backtoblog a, .login #nav a {color: #000;}
    .wp-core-ui .button-primary {
	    background: #000;
	    border-color: #000;
	    -webkit-box-shadow: 0 1px 0 #00afec;
	    box-shadow: none !important;
	    color: #fff;
	    text-decoration: none;
	    text-shadow: none !important;
	}
	.wp-core-ui .button-primary {
	    background: #000 !important;
	    border-color: #000 !important;
	    -webkit-box-shadow: none !important;
	    box-shadow: none !important;
	}
	.wp-core-ui .button-primary.focus, .wp-core-ui .button-primary.hover, .wp-core-ui .button-primary:focus, .wp-core-ui .button-primary:hover {
	    background: red !important;
	    border-color: red !important;
	    color: #fff;
    	box-shadow: none !important;
	}

    </style>';
}

add_action('login_head', 'custom_login_logo');

add_action('admin_head', 'show_favicon');
function show_favicon() {?>
    <link rel="shortcut icon" type="image/x-icon" href="<?php bloginfo('template_url')?>/images/favicon.ico" />
<?php }

add_action( 'customize_register', 'prefix_remove_css_section', 15 );
/**
 * Remove the additional CSS section, introduced in 4.7, from the Customizer.
 * @param $wp_customize WP_Customize_Manager
 */
function prefix_remove_css_section( $wp_customize ) {
	$wp_customize->remove_section( 'custom_css' );
}
function remove_editor_menu() {
  remove_action('admin_menu', '_add_themes_utility_last', 101);
}
add_action('_admin_menu', 'remove_editor_menu', 1);
function create_posttype() {
	register_post_type( 'top-slides',
    array(
      'labels' => array(
        'name' => __( 'Top Slides' ),
        'singular_name' => __( 'Top Slide' )
      ),
			'supports' => array( 'title', 'editor', 'thumbnail' ),
      'public' => true,
			'has_archive' => false
    )
  );
	register_post_type( 'footer-address',
    array(
      'labels' => array(
        'name' => __( 'Footer Address' ),
        'singular_name' => __( 'Footer Address' )
      ),
			'supports' => array( 'title', 'editor' ),
      'public' => true,
			'has_archive' => false
    )
  );
}
add_action( 'init', 'create_posttype' );
add_filter('nav_menu_css_class' , 'special_nav_class' , 10 , 2);
function special_nav_class ($classes, $item) {
    if (in_array('current-page-ancestor', $classes) || in_array('current-menu-item', $classes) ) {
        $classes[] = 'active_nav_link';
    }
    return $classes;
}
function process_form() {
	 $fullname = strip_tags(trim($_POST["fullName"]));
	 $user_email = filter_var(trim($_POST["email"]), FILTER_SANITIZE_EMAIL);
	 $user_input_subject = strip_tags(trim($_POST["subject"]));
	 $user_message = strip_tags(trim($_POST["message"]));
	 $user_ip = $_SERVER["REMOTE_ADDR"];
	 $current_date = date("Y-m-d, H:i:s");
	 $header = 'From: M&RK Advocates info@mrkadvocates.com' . "\r\n";
	 $header .= "MIME-Version: 1.0" . "\r\n";
	 $header .= "Content-type:text/html;charset=utf-8" . "\r\n";
	 $admin_email = "markoj.services@gmail.com";
	 $user_subject = 'Contact Form';
	 $admin_subject = 'New Contact Form Submission';
	 $user_message_styled = '<div style="background:white;padding:20px 0px 0px 0px;"><br/><br/>
		<div style="width:100%;display:block;font-family:Verdana;">Dear admin,</div><br/>
		<div style="width:100%;display:block;font-family:Verdana;">Kindly note that a new contact form has been received. Find details below:</div><br/>
		<div style="width:100%;display:block;font-family:Verdana;line-height:20px;"><strong style="color:red;">Name:</strong> '.$fullname.'</div>
		<div style="width:100%;display:block;font-family:Verdana;line-height:20px;"><strong style="color:red;">E-mail Address:</strong> '.$user_email.'</div>
		<div style="width:100%;display:block;font-family:Verdana;line-height:20px;"><strong style="color:red;">Subject:</strong> '.$user_input_subject.'</div>
		<div style="width:100%;display:block;font-family:Verdana;line-height:20px;white-space: pre-wrap;"><strong style="color:red;">Message:</strong> '.$user_message.'</div>
		<br/>
		<br/>
		<a href="http://mrkadvocates.com/" target="_blank"><img src="http://mrkadvocates-new.ipoint.com.mt/images/logo.png" alt="mrk advocates logo"></a>
		</div>';
	 $admin_message = '<div style="background:white;padding:20px 0px 0px 0px;"><br/>
		<div style="width:100%;display:block;font-family:Verdana;">Dear '.$fullname.',</div><br/>
		<div style="width:100%;display:block;font-family:Verdana;">Thank you for contacting us. We will get back to you shortly.</div><br/><br/>
		<a href="http://mrkadvocates.com/" target="_blank"><img src="http://mrkadvocates-new.ipoint.com.mt/images/logo.png" alt="mrk advocates logo"></a>
		</div>';
	 $host = "";
	 $username = "";
	 $password = "";
	 $database = "";
	 $dbhandle = mysqli_connect($host, $username, $password);
	 mysqli_select_db($dbhandle, $database);
	 mysqli_query($dbhandle, "INSERT INTO contact_messages(fullname, email, subject, message, ipaddress, insertion_date) VALUES ('$fullname', '$user_email', '$user_input_subject', '$user_message', '$user_ip', '$current_date')");
	 mysqli_close($dbhandle);
	 mail($user_email, $admin_subject, $admin_message, $header);
	 if (mail($admin_email, $user_subject, $user_message_styled, $header)) {
		echo "Your message has been sent. Thank you!";
	 } else {
		echo "Something went wrong on our end and we couldn't send your message.";
	 }
	 die();
}
add_action("wp_ajax_nopriv_mrk_process_form", "process_form");
