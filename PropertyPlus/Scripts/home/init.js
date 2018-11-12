
function loadApartmentDetail() {
        $('.tour-this-home .owl-carousel').owlCarousel({
            margin: 30,
            autoplay: true,
            autoplayTimeout: 2500,
            autoplayHoverPause: true,
            loop: true,
            items: 2,
            nav: true,
            animateOut: 'fadeOut'
        });

        // $('.header-apartment-detail .owl-carousel').owlCarousel({
        //     margin: 0,
        //     autoplay: true,
        //     autoplayTimeout: 2500,
        //     autoplayHoverPause: true,
        //     loop: true,
        //     items: 1,
        //     nav: true,
        //     animateOut: 'fadeOut'
        // });

        $('.similar-listing .owl-carousel').owlCarousel({
            margin: 0,
            autoplay: true,
            autoplayTimeout: 2500,
            autoplayHoverPause: true,
            loop: true,
            items: 4,
            nav: true,
            animateOut: 'fadeOut'
        });
        // $('.similar-listing .thumbnail-choose-home .owl-carousel').owlCarousel({
        //     margin: 0,
        //     autoplay: true,
        //     autoplayTimeout: 2500,
        //     autoplayHoverPause: true,
        //     loop: true,
        //     items: 1,
        //     nav: true,
        //     animateOut: 'fadeOut'
        // });
    }
$(document).ready(function(){
    $(".btn-scroll-top").click(function(){
        $(window).scrollTop(0);
    });
});