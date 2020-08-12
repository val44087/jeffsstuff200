﻿using LibraryApi;
using LibraryApi.Domain;
using LibraryApi.Services;
using LibraryApiIntegrationTests.Fakes;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibraryApiIntegrationTests
{
    public class WebTestFixture : WebApplicationFactory<Startup>
    {
        public LibraryDataContext Context;
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");

            builder.ConfigureServices(services =>
            {
                // reach into the configured services and find the one for ISystemTime
                var systemTimeDescriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(ISystemTime));

                if(systemTimeDescriptor != null)
                {
                    services.Remove(systemTimeDescriptor);
                    services.AddTransient<ISystemTime, TestingSystemTime>();
                }

                var cacheDescriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(ILookupDevelopers));
                if(cacheDescriptor != null)
                {
                    services.Remove(cacheDescriptor);
                    services.AddTransient<ILookupDevelopers, TestingDeveloperCache>();
                }


                var dbContextDescriptor = services.SingleOrDefault(d =>
                    d.ServiceType == typeof(DbContextOptions<LibraryDataContext>)
                );

                if(dbContextDescriptor != null)
                {
                    services.Remove(dbContextDescriptor);
                    services.AddDbContext<LibraryDataContext>(options =>
                    {
                        options.UseInMemoryDatabase("JustAUniqueName");
                    });
                    
                    var sp = services.BuildServiceProvider();

                    using (var scope = sp.CreateScope())
                    {
                        var scopedServices = scope.ServiceProvider;
                        var db = scopedServices.GetRequiredService<LibraryDataContext>();
                        // db.Database.EnsureDeleted();
                        if (db.Database.EnsureCreated())
                        {
                            DataUtils.Initialize(db);

                        } 
                    }
                }

            });


        }
    }
}
