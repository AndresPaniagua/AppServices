using AppServices.Common.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace AppServices.Prism.Views
{
    public partial class ServiceDetailsPage : ContentPage
    {
        private static ServiceDetailsPage _instance;

        public ServiceDetailsPage()
        {
            InitializeComponent();
            _instance = this;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        public static ServiceDetailsPage GetInstance()
        {
            return _instance;
        }

        public async void DrawMap(ServiceResponse service)
        {
            try
            {
                Geocoder geoCoder = new Geocoder();
                IEnumerable<Position> sources = await geoCoder.GetPositionsForAddressAsync(service.User.Address);
                List<Position> positions = new List<Position>(sources);

                if (positions.Count > 0)
                {
                    Position positionTot = positions[0];

                    if (positionTot.Latitude != 0 && positionTot.Longitude != 0)
                    {
                        AddPin(positionTot, service.User.Address, "Service Place", PinType.Place);
                        MoveMap(positionTot);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            
        }

        public void AddPin(Position position, string address, string label, PinType pinType)
        {
            MyMap.Pins.Add(new Pin
            {
                Address = address,
                Label = label,
                Position = position,
                Type = pinType
            });
        }

        private void MoveMap(Position position)
        {
            try
            {
                MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(position, Distance.FromKilometers(.05)));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }
}
