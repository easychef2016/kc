(function (app) {
    'use strict';

    app.controller('restaurantRegisterCtrl', restaurantRegisterCtrl);

    restaurantRegisterCtrl.$inject = ['$scope', '$state', 'membershipService', 'notificationService', '$rootScope', '$location', 'apiService'];

    function restaurantRegisterCtrl($scope, $state, membershipService, notificationService, $rootScope, $location, apiService) {

        $scope.markers = [];
        $scope.result1 = '';
        $scope.options1 = null;
        $scope.details1 = '';


        $scope.showUserNameExists = false;
        $scope.showUserNameAvailable = false;

        $scope.checkUserAvailable = checkUserAvailable;
        $scope.ClearMessages = ClearMessages;



        $scope.user = {};
        $scope.loginuser = {
            Username: '',
            Password: ''
        }


        $scope.pageClass = 'page-login';
        $scope.register = register;
        // alert("test");
        //$scope.user = {};
        $scope.restaurantuser = {};
        // alert($scope.restaurantuser.UserEmail);


        $scope.restaurantuseraddressvm = {

            RestaurantAddressVM: [{
                AddressDetails: '',
                StreetName: ''

            }],
            SubcriptionVM: [{
                ID: 1003,
                Price: 0.0
            }]
        }
        toastr.options = {
            "debug": false,
            "positionClass": "toast-top-right",
            "onclick": null,
            "fadeIn": 300,
            "fadeOut": 1000,
            "timeOut": 3000,
            "extendedTimeOut": 1000,
            "progressBar": true
        };


        function ClearMessages() {
            $scope.showUserNameExists = false;
            $scope.showUserNameAvailable = false;
        }

        function checkUserAvailable() {

            $scope.showUserNameExists = false;
            $scope.showUserNameAvailable = false;


            $scope.loginuser.Username = $scope.restaurantuseraddressvm.RestaurantUserVM.Username;
            apiService.post('/api/account/checkusername', $scope.loginuser,
                checkUserAvailableCompleted,
                checkUserAvailableFailed);
        }

        function checkUserAvailableCompleted(response) {
            if (response.status == '406') {
                $scope.showUserNameExists = true;
                $scope.showUserNameAvailable = false;
                $scope.userRegistrationForm.UserName.$setValidity('unique', true);
                //notificationService.displayWarning("Email address/User name already exists!")
                toastr.warning("Email address/User name already ");
            }
            else if (response.status == '200') {
                $scope.showUserNameAvailable = true;
                $scope.showUserNameExists = false;
                $scope.userRegistrationForm.UserName.$setValidity('unique', true);
                // notificationService.displaySuccess("Email address /User Name is available")
            }
            else {
                notificationService.displayError(response.statusText);
            }


        }



        function checkUserAvailableFailed(response) {
            console.log(response);
            if (response.status == '406') {
                $scope.showUserNameExists = true;
                $scope.showUserNameAvailable = false;
                $scope.userRegistrationForm.UserName.$setValidity('unique', true);
                // notificationService.displayWarning("Email address/User name already exists!")
                toastr.warning("Email address/User name already exists");
            }
            else if (response.status == '200') {
                $scope.showUserNameAvailable = true;
                $scope.showUserNameExists = false;
                $scope.userRegistrationForm.UserName.$setValidity('unique', true);
                // notificationService.displaySuccess("Email address /User Name is available")

            }
            else {
                // notificationService.displayError(response.statusText);
            }

        }





        function register() {
            // alert("test1");
            $scope.restaurantuseraddressvm.SubcriptionVM.ID = 1003;
            membershipService.register($scope.restaurantuseraddressvm, registerCompleted)
        }

        function registerCompleted(result) {
            // if (result.data.success) {
            console.log(result);
            if (result.data) {
                $scope.user = result.data;
                $scope.user.Password = $scope.restaurantuseraddressvm.RestaurantUserVM.Password;
                membershipService.saveCredentials($scope.user);

                //-----------------------------------------------------
                // notificationService.displaySuccess('Hello ' + $scope.user.Username);
                //  $scope.userData.displayUserInfo();              
                // $state.go('Management.Profile');
                //-----------------------------------------------------

                $state.go("welcome");


            }
            else {


                notificationService.displayError('Registration failed for : ' + result.statusText + '. Try again.');
            }
        }
    }

})(angular.module('common.core'));