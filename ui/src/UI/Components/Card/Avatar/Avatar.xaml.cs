﻿// <copyright file="Avatar.xaml.cs" company="Mozilla">
// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0. If a copy of the MPL was not distributed with this file, you can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Runtime.Caching;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FirefoxPrivateNetwork.UI.Components
{
    /// <summary>
    /// Interaction logic for Avatar.xaml.
    /// </summary>
    public partial class Avatar : UserControl
    {
        /// <summary>
        /// Dependency property for the profile picture image source.
        /// </summary>
        public static readonly DependencyProperty AvatarImageProperty = DependencyProperty.Register("Url", typeof(ImageSource), typeof(Avatar));

        /// <summary>
        /// Dependency property for the avatar size.
        /// </summary>
        public static readonly DependencyProperty SizeProperty = DependencyProperty.Register("Size", typeof(double), typeof(Avatar));

        /// <summary>
        /// Initializes a new instance of the <see cref="Avatar"/> class.
        /// </summary>
        public Avatar()
        {
            InitializeComponent();
            AvatarImage = GetProfileImage();
        }

        /// <summary>
        /// Gets the application version.
        /// </summary>
        public string ApplicationVersion
        {
            get
            {
                return ProductConstants.GetVersion();
            }
        }

        /// <summary>
        /// Gets or sets the profile picture image source.
        /// </summary>
        public ImageSource AvatarImage
        {
            get
            {
                return GetProfileImage();
            }

            set
            {
                SetValue(AvatarImageProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the avatar size.
        /// </summary>
        public double Size
        {
            get
            {
                return (double)GetValue(SizeProperty);
            }

            set
            {
                SetValue(SizeProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the avatar image.
        /// </summary>
        /// <returns>Image source.</returns>
        public ImageSource GetProfileImage()
        {
            if (Manager.Account.LoginState == FxA.LoginState.LoggedIn)
            {
                if (Manager.Account.Config.FxALogin.User.Avatar != null)
                {
                    var image = Manager.Cache.Get("avatarImage");

                    if (image != null)
                    {
                        return (BitmapImage)image;
                    }
                    else
                    {
                        CacheItemPolicy policy = new CacheItemPolicy();

                        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(Manager.Account.Config.FxALogin.User.Avatar);

                        try
                        {
                            using (HttpWebResponse response = (HttpWebResponse)req.GetResponse())
                            {
                                image = new BitmapImage(new Uri(Manager.Account.Config.FxALogin.User.Avatar));
                            }

                            Manager.Cache.Set("avatarImage", image, policy);
                            return (BitmapImage)image;
                        }
                        catch (Exception e)
                        {
                            ErrorHandling.ErrorHandler.Handle(e, ErrorHandling.LogLevel.Debug);
                        }
                    }
                }
            }

            return new BitmapImage(new Uri(System.IO.Path.Combine(Environment.CurrentDirectory, @"..\..\..\src\UI\Resources\Icons\Generic\default-avatar.png")));
        }

        private void NavigateSettings(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;

            var parentView = this.Parent;
            while (!(parentView is UserControl))
            {
                parentView = LogicalTreeHelper.GetParent(parentView);
            }

            mainWindow.NavigateToView(new SettingsView(parentView as UserControl), MainWindow.SlideDirection.Left);
        }

        private void ButtonViewLogClick(object sender, RoutedEventArgs e)
        {
            LogWindow.ShowLog();
            AvatarMenu.IsOpen = false;
        }

        private void ButtonFeedbackClick(object sender, RoutedEventArgs e)
        {
            Process.Start(ProductConstants.FeedbackFormUrl);
            AvatarMenu.IsOpen = false;
        }

        private void ButtonManageAccountClick(object sender, RoutedEventArgs e)
        {
            Process.Start(ProductConstants.FxAAccountManagementUrl);
            AvatarMenu.IsOpen = false;
        }

        private void ButtonSettingsClick(object sender, RoutedEventArgs e)
        {
            AvatarMenu.IsOpen = false;
            NavigateSettings(sender, e);
        }

        private void ButtonDebugClick(object sender, RoutedEventArgs e)
        {
            AvatarMenu.IsOpen = false;

            using (var saveDialog = new System.Windows.Forms.SaveFileDialog
            {
                Filter = "ZIP Archive|*.zip",
                Title = "Export debug package",
            })
            {
                if (MessageBox.Show("Thank you for debugging the Firefox Private Network VPN client!\n\nThis utility will export a ZIP file to a directory of your choosing." +
                    " This file will contain the following:\n\n- A list of your running processes\n- A list of your devices and device drivers\n" +
                    "- Information about your network interfaces\n- Your computer hardware information\n\n" +
                    "Along with the VPN tunnel log, the currently available list of VPN servers will also be included.\n" +
                    "Your Firefox account information and any of your VPN credentials will not be exported.\n\n" +
                    "Do you wish to proceed?",
                    "Privacy notice",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question
                    ) == MessageBoxResult.Yes)
                {
                    saveDialog.ShowDialog();

                    if (saveDialog.FileName != string.Empty)
                    {
                        ErrorHandling.DebugDump.CreateDump(saveDialog.FileName);
                    }
                }
            }
        }

        private void ButtonSignOutClick(object sender, RoutedEventArgs e)
        {
            Manager.Account.Logout(removeDevice: true);

            AvatarMenu.IsOpen = false;

            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.NavigateToView(new LandingView(), MainWindow.SlideDirection.Right);
        }
    }
}
