﻿using KeShMovies.Views;
using Autofac;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using KeShMovies.ViewModels;
using KeShMovies.Navigation;
using KeShMovies.Repositories;

namespace KeShMovies;

public partial class App : Application
{
    private void Application_Startup(object sender, StartupEventArgs e)
    {
        NavigationStore navigationStore = new();

        var builder = new ContainerBuilder();

        builder.RegisterInstance(navigationStore).SingleInstance();

        builder.RegisterType<MainViewModel>();
        builder.RegisterType<HomeViewModel>();
        builder.RegisterType<SignUpViewModel>();
        builder.RegisterType<LogInViewModel>();
        builder.RegisterType<EfUserRepository>().As<IUserRepository>();
        

        var container = builder.Build();

        navigationStore.CurrentViewModel = container.Resolve<LogInViewModel>();

        MainView mainView = new();
        mainView.DataContext= container.Resolve<MainViewModel>();



        mainView.Show();
    }
}
