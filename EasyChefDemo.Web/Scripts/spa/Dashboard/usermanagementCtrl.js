(function (app) {
    'use strict';

    app.controller('usermanagementCtrl', usermanagementCtrl)

    //usermanagementCtrl.$inject = ['$scope', '$location', '$timeout', 'apiService', 'notificationService', '$rootScope'];
    usermanagementCtrl.$inject = ['$scope', '$timeout', 'apiService', 'notificationService', '$rootScope'];
    //function usermanagementCtrl($scope, $location, $timeout, apiService, notificationService, $rootScope) {
    function usermanagementCtrl($scope, $timeout, apiService, notificationService, $rootScope) {
        $scope.filterUsername = '';
        $scope.restaurantdetails = {};
        $scope.subscriptiondetails = {};
        $scope.subscriptionplans = [];
        $scope.restaurantusers = {};
        $scope.latestvendors = [];
        $scope.selecteddata = {};
        $scope.loadData = loadData;

        $scope.loadUserdata = loadUserdata;
        $scope.loadrestaurantdetails = loadrestaurantdetails;
        $scope.loadsubscriptiondetails = loadsubscriptiondetails;
        $scope.loadsubscriptionplan = loadsubscriptionplan;
        $scope.loadrestuarantusers = loadrestuarantusers;
        $scope.PaymentInfo = PaymentInfo;
        $scope.UserItem = UserItem;
        // $scope.status = true;

        //$scope.paymentVM = {};
        function initializeUserItems() {
            $scope.UserInfo = {
                ID: ''
            }
        }

        initializeUserItems();
        $scope.paymentVM = {

            BillingAddressVM: [{
                AddressDetails: '',
                StreetName: ''

            }],
            loginUserInfo: '',
            ID: '',
            Amount: 0.0
        }
        $scope.PaymentItemList = {};
        $scope.UserList = {};


        $scope.submitted = false;
        $scope.submitting = false;
        //$scope.PaymentItems = [];

        function loadUserdata() {

            if ($rootScope.repository.loggedUser) {
                $scope.filterUsername = $rootScope.repository.loggedUser.username;
                $scope.paymentVM.sloginUserInfo = $scope.filterUsername;
            }
        }

        function loadrestaurantdetails() {

            var config = {
                params: {
                    filter: $scope.filterUsername
                }
            };
            apiService.get('api/restaurant/details/', config, loadrestaurantdetailsCompleted, loadrestaurantdetailsFailed);
        }

        function loadrestaurantdetailsCompleted(response) {

            $scope.restaurantdetails = response.data;
            $scope.UserInfo.ID = $scope.restaurantdetails.ID;
        }

        function loadrestaurantdetailsFailed(response) {
            consol.log(response);
        }

        $scope.UpdateRestaurantDetails = function () {
            $scope.selecteddata = angular.copy($scope.restaurantdetails);
            apiService.post('/api/restaurant/update', $scope.restaurantdetails,
                updateSucceded,
                updateFailed);
        };

        function updateSucceded(response) {
            console.log(response);
            notificationService.displaySuccess('Restaurant has been updated');
            loadrestaurantdetails();

        }

        function updateFailed(response) {
            notificationService.displayError(response);
        }

        $scope.ResetBack = function () {
            $scope.restaurantdetails = "";
            loadrestaurantdetails();
        };


        function loadrestuarantusers() {
            $scope.restaurantusers = {};
            var config = {
                params: {
                    filter: $scope.filterUsername
                }
            };
            apiService.get('api/Users/details', config, loadrestuarantusersCompleted, loadrestuarantusersFailed);
        }

        function loadrestuarantusersCompleted(response) {
            $scope.restaurantusers = {};
            $scope.restaurantusers = response.data;
        }

        function loadrestuarantusersFailed() {
            console.log(response);
        }

        function UserItem() {
            $scope.submitted = true;

            if ($scope.AddUserForm.$valid) {
                $scope.submitting = true;
                var UserList = {};
                UserList = angular.copy($scope.UserInfo);
                apiService.post('/api/Users/add', UserList,
                    UserItemSucceded,
                    UserItemFailed);


            } else {
                $scope.UserErrorMessage = "Please enter username as email and password for creating users"
            }

        }


        function UserItemSucceded(response) {
            notificationService.displaySuccess('User Added ');
            $scope.UserErrorMessage = "";
            initializeUserItems();
            $scope.loadrestuarantusers = ""
            $scope.restaurantusers = {};
            loadrestuarantusers();
            $scope.submitting = false;


        }

        function UserItemFailed(response) {
            // notificationService.displayError(response);
            $scope.UserErrorMessage = response.data;
            $scope.submitting = false;
        }


        $scope.changeStatus = function (users) {

            $scope.username1 = angular.copy(users);
            apiService.post('/api/Users/update', users, usersupdateCompleted, usersupdateFailed);
        }
        function usersupdateCompleted(response) {
            console.log(response);
            alert('hi');
            notificationService.displaySuccess('User has been updated');
            $scope.loadrestuarantusers = ""
            $scope.restaurantusers = {};
            loadrestuarantusers();
        }

        function usersupdateFailed(response)
        { notificationService.displayError(response); }

        function loadsubscriptiondetails() {
            var config = {
                params: {
                    filter: $scope.filterUsername
                }
            };
            apiService.get('api/Payments/details/', config, loadsubscriptiondetailsCompleted, loadsubscriptiondetailsFailed);
        }

        function loadsubscriptiondetailsCompleted(response) {
            $scope.subscriptiondetails = response.data;
        }

        function loadsubscriptiondetailsFailed(response) {
            console.log(response);
        }


        function loadsubscriptionplan() {

            apiService.get('api/Payments/plans', null, loadsubscriptionplanCompleted, loadsubscriptionplanFailed);
        }

        function loadsubscriptionplanCompleted(response) {
            //  alert(response.data);
            $scope.subscriptionplans = response.data;
        }

        function loadsubscriptionplanFailed(response) {
            console.log(response);
        }





        $scope.DropDownChnaged = function (item) {
            if (item != null) {
                $scope.DropDownStatus = item.Price;
                $scope.paymentVM.iPlanID = item.ID;
                $scope.paymentVM.dAmount = item.Price;
            } else {
                $scope.DropDownStatus = 0;
                $scope.paymentVM.iPlanID = 0;
                $scope.paymentVM.dAmount = 0;
            }
        };



        function PaymentInfo() {

            $scope.submitted = true;
            if ($scope.PaymentItemForm.$valid) {
                $scope.submitting = true;

                var PaymentItemList = {};
                PaymentItemList = angular.copy($scope.paymentVM);

                apiService.post('/api/Payments/addPay', PaymentItemList,
                    PaymentInfoSucceded,
                    PaymentInfoFailed);
            }

        }

        function PaymentInfoSucceded(response) {
            notificationService.displaySuccess('Payment has been submitted ');
            $scope.ErrorMessage = "";
            $scope.Message = response.data.messages[0].description + "/n" + "Please note TransactionID for reference :" + response.data.transId;
            $scope.submitting = false;


        }

        function PaymentInfoFailed(response) {
            notificationService.displayError(response.data);
            $scope.Message = "";
            $scope.ErrorMessage = response.data[0].errorText;
            $scope.submitting = false;
        }




        function loadData() {

            var config = {
                params: {

                    filter: $scope.filterUsername
                }
            };

            apiService.get('/api/vendors/latest', config,
                vendorsLoadCompleted,
                vendorsLoadFailed);

        }

        function vendorsLoadCompleted(result) {
            $scope.latestvendors = result.data;
            $scope.loadingvendors = false;
        }


        function vendorsLoadFailed(response) {
            notificationService.displayError(response.data);
        }
        //$scope.status = true;


        loadUserdata();
        loadrestaurantdetails();
        loadsubscriptiondetails();
        loadsubscriptionplan();
        loadrestuarantusers();
        loadData();

    }


})(angular.module('easychefdemo'));