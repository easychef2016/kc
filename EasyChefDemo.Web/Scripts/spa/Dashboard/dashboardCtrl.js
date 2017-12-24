(function (app) {
    'use strict';

    app.controller('dashboardCtrl', dashboardCtrl);

    dashboardCtrl.$inject = ['$scope', '$location', '$timeout', 'apiService', 'notificationService', '$rootScope'];

    function dashboardCtrl($scope, $location, $timeout, apiService, notificationService, $rootScope) {


        $scope.filterUsername = '';
        $scope.restaurantdetails = {};
        $scope.loadrestaurantdetails = loadrestaurantdetails;
        $scope.loadUserdata = loadUserdata;
        $scope.loadingRestaurant = true;

        $scope.dashboarddetails = {};

        function loadUserdata() {

            if ($rootScope.repository.loggedUser) {
                $scope.filterUsername = $rootScope.repository.loggedUser.username;
                
            }
        }
        

        function loadrestaurantdetails() {
            var config = {
                params: {

                    filter: $scope.filterUsername
                }
            };
            apiService.get('/api/restaurant/details/', config,
            loadrestaurantdetailsCompleted,
            loadrestaurantdetailsFailed);
        }
        

        function loadrestaurantdetailsCompleted(response) {
            //  alert(response.data);
            $scope.restaurantdetails = response.data;
            $scope.loadingRestaurant = false;
        }

        function loadrestaurantdetailsFailed(response) {
            console.log(response);
            //notificationService.displayError(response.statusText);
        }



        function loadDashboardData() {
           
            var config = {
                params: {

                    filter: $scope.filterUsername
                }
            };

            apiService.get('/api/inventories/dashboard/', config,
                      loadDashboardDataCompleted,
                        loadDashboardDataFailed);

        }

        function loadDashboardDataCompleted(result) {
            $scope.dashboarddetails = result.data;
        }

     
        function loadDashboardDataFailed(response) {
          
            notificationService.displayError(response.data);
        }

        //Added on 12/09/2017
        $scope.loadinginventories = true;
        $scope.latestinventories = [];
        $scope.rowCollection = [];
        $scope.loadinventoryItems = loadinventoryItems;

        $scope.getStatustext = getStatustext;
        function getStatustext(status) {
            // alert("test");
            if (status == 'True' || status == true)
                return 'Active';
            else {
                return 'In Active';
            }
        }

        $scope.getStatustextVendor = getStatustextVendor;
        function getStatustextVendor(status) {           
            if (status == 1)
                return 'Submitted to Vendor';
            else {
                return 'Created';
            }
        }



       
        function loadinventoryItems() {
            $scope.filterUsername = $rootScope.repository.loggedUser.username;
            var config = {
                params: {
                    filter: $scope.filterUsername
                }
            };
            apiService.get('/api/inventories/latest/', config,
                           inventoryItemsLoadCompleted,
                            inventoryItemsLoadFailed);
         
        }
        function inventoryItemsLoadCompleted(result) {
            $scope.latestinventories = result.data;
            $scope.rowCollection = result.data;
            $scope.loadinginventories = false;
        }       

        function inventoryItemsLoadFailed(response) {
                notificationService.displayError(response.data);
        }

        function gotoInventorySheet() {
            $state.go("Management.inventorydetails");


        }


      
        loadUserdata();
        loadrestaurantdetails();
        loadDashboardData();
        loadinventoryItems();


    }

})(angular.module('easychefdemo'));