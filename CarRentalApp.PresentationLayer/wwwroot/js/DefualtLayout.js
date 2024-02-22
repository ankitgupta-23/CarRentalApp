$(document).ready(() => {

    $("#top-div-text-1").waypoint((direction) => {


        $("#top-div-text-1").addClass('animate__animated animate__lightSpeedInLeft');
        $("#top-div-text-2").addClass('animate__animated animate__fadeInUp animate__delay-1s');
        $("#call-to-action-btn").addClass('animate__animated animate__jackInTheBox animate__delay-2s');

    }, { offset: '100%' });

    $("#steps-heading").waypoint((direction) => {
        $("#steps-heading").addClass('animate__animated animate__fadeInUp')
        $("#step1_div").addClass('animate__animated animate__bounceInLeft animate__delay-1s')
        $("#step2_div").addClass('animate__animated animate__fadeInUp animate__delay-1s')
        $("#step3_div").addClass('animate__animated animate__bounceInRight animate__delay-1s')

    }, { offset: '400' });

    $("#promo-heading").waypoint((direction) => {
        $("#promo-heading").addClass('animate__animated animate__jackInTheBox');
        $("#promo-col-left-1").addClass('animate__animated animate__fadeInUp animate__delay-1s');
        $("#promo-col-right-1").addClass('animate__animated animate__fadeInRight animate__delay-1s');
        $("#promo-col-left-2").addClass('animate__animated animate__fadeInUp animate__delay-2s');
        $("#promo-col-right-2").addClass('animate__animated animate__fadeInRight animate__delay-2s');
        $("#promo-col-left-3").addClass('animate__animated animate__fadeInUp animate__delay-3s');
        $("#promo-col-right-3").addClass('animate__animated animate__fadeInRight animate__delay-3s');
    }, { offset: '350' });




    setInterval(() => {
        $("#top-div-text-1").toggleClass("change-color");
    }, 1000);

});