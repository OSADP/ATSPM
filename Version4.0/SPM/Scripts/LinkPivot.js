﻿//angular.module('plunker', ['ui.bootstrap', 'ui.bootstrap.datetimepicker']);

//angular.module('plunker').controller('DateTimePickerDemoCtrl',
//function ($scope, $timeout) {
//    $scope.dateTimeNow = function () {
//        $scope.date = new Date();
//    };
//    $scope.dateTimeNow();

//    $scope.toggleMinDate = function () {
//        var minDate = new Date();
//        // set to yesterday
//        minDate.setDate(minDate.getDate() - 1);
//        $scope.minDate = $scope.minDate ? null : minDate;
//    };

//    $scope.toggleMinDate();

//    $scope.dateOptions = {
//        showWeeks: false
//    };

//    // Disable weekend selection
//    $scope.disabled = function (calendarDate, mode) {
//        return mode === 'day' && (calendarDate.getDay() === 0 || calendarDate.getDay() === 6);
//    };

//    $scope.open = function ($event, opened) {
//        $event.preventDefault();
//        $event.stopPropagation();
//        $scope.dateOpened = true;
//    };

//    $scope.dateOpened = false;
//    $scope.hourStep = 1;
//    $scope.format = "dd-MMM-yyyy";
//    $scope.minuteStep = 15;

//    $scope.timeOptions = {
//        hourStep: [1, 2, 3],
//        minuteStep: [1, 5, 10, 15, 25, 30]
//    };

//    $scope.showMeridian = true;
//    $scope.timeToggleMode = function () {
//        $scope.showMeridian = !$scope.showMeridian;
//    };

//    $scope.$watch("date", function (date) {
//        // read date value
//    }, true);

//    $scope.resetHours = function () {
//        $scope.date.setHours(1);
//    };
//});