$.noConflict();

jQuery(document).ready(function ($) {

    "use strict";

    [].slice.call(document.querySelectorAll('select.cs-select')).forEach(function (el) {
        new SelectFx(el);
    });

    jQuery('.selectpicker').selectpicker;




    $('.search-trigger').on('click', function (event) {
        event.preventDefault();
        event.stopPropagation();
        $('.search-trigger').parent('.header-left').addClass('open');
    });

    $('.search-close').on('click', function (event) {
        event.preventDefault();
        event.stopPropagation();
        $('.search-trigger').parent('.header-left').removeClass('open');
    });

    $('.equal-height').matchHeight({
        property: 'max-height'
    });

    // Counter Number
    $('.count').each(function () {
        $(this).prop('Counter', 0).animate({
            Counter: $(this).text()
        }, {
            duration: 3000,
            easing: 'swing',
            step: function (now) {
                $(this).text(Math.ceil(now));
            }
        });
    });


    // Menu Trigger
    $('#menuToggle').on('click', function (event) {
        var windowWidth = $(window).width();
        var isMenuOpen;

        if (windowWidth < 1010) {
            $('body').removeClass('open');
            if (windowWidth < 760) {
                $('#left-panel').slideToggle();
            } else {
                $('#left-panel').toggleClass('open-menu');
            }
            isMenuOpen = $('body').hasClass('open');
        } else {
            $('body').toggleClass('open');
            $('#left-panel').removeClass('open-menu');
            isMenuOpen = $('body').hasClass('open');
        }

        localStorage.setItem('isMenuOpen', isMenuOpen);
    });



    $(".dropdown-toggle").on("click", function () {
        var dropdown = $('.menu-item-has-children');
        var dropdownWithShow = dropdown.filter('.show');
        var dropdownToggleText = dropdownWithShow.children('a.dropdown-toggle').eq(0).text();
        
        var subMenuToShow = dropdownWithShow.find('ul.sub-menu.children.dropdown-menu.show');
        var subtitleExists = subMenuToShow.find('.subtitle').length > 0;

        subMenuToShow.css({
            'background-color': 'white'
        });

        if (!subtitleExists) {
            subMenuToShow.prepend('<li class="subtitle">' + dropdownToggleText + '</li>');
        }
    });

    // Load Resize 
    $(window).on("load resize", function (event) {       
        var windowWidth = $(window).width();
        if (windowWidth < 1010) {
            $('body').addClass('small-device');
        } else {
            $('body').removeClass('small-device');
        }

    });

    // Pegar Enums
    function getEnumDisplayName(enumType, enumValue) {
        var enumEntries = Object.entries(enumType);
        for (var i = 0; i < enumEntries.length; i++) {
            var key = enumEntries[i][0];
            var value = enumEntries[i][1];
            if (typeof value === 'number' && value === enumValue) {
                return key;
            }
        }
        return "";
    }



});