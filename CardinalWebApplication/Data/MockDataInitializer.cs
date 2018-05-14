using CardinalLibrary;
using CardinalWebApplication.Extensions;
using CardinalWebApplication.Models.DbContext;
using CardinalWebApplication.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CardinalWebApplication.Data
{
    public class MockDataInitializer
    {
        private ApplicationDbContext _context { get; set; }
        private UserManager<ApplicationUser> _userManager { get; set; }
        private IHexagonal _hexagonal { get; set; }
        private ILocationHistoryService _locationHistoryService { get; set; }

        public async Task InitializeMockUsers(IServiceProvider serviceProvider)
        {
            _userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            _context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            _hexagonal = serviceProvider.GetRequiredService<IHexagonal>();
            _locationHistoryService = serviceProvider.GetRequiredService<ILocationHistoryService>();

            var option = await _context.ApplicationOptions
                                       .OrderByDescending(a => a.OptionsDate)
                                       .FirstOrDefaultAsync();
            if(option == null)
            {
                var appOption = new ApplicationOption()
                {
                    OptionsDate = DateTime.Now.ToUniversalTime(),
                    DataTimeWindow = TimeSpan.FromHours(12),
                    EndUserLicenseAgreementSource = "http://www.google.com/",
                    TermsConditionsSource = "http://www.google.com/",
                    PrivacyPolicySource = "http://www.google.com/",
                    Version = 1,
                    VersionMajor = 0,
                    VersionMinor = 0
                };
                await _context.ApplicationOptions
                              .AddAsync(appOption);
                await _context.SaveChangesAsync();
            }

            //create the mock users if they don't exist
            var mock = await _userManager.FindByEmailAsync("Mock01@RyanRauch.com");
            if (mock == null)
            {
                for (int i = 1; i <= 25; ++i)
                {
                    string mockFirst = String.Format("Mock{0}", i.ToString("D2"));
                    string mockLast = String.Format("Data{0}", i.ToString("D2"));
                    string mockMail = String.Format("{0}@RyanRauch.com", mockFirst);
                    string mockNumber = String.Format("55512300{0}", i.ToString("D2"));
                    string mockPass = String.Format("Password{0}!", i.ToString("D2"));
                    var user = new ApplicationUser
                    {
                        UserName = mockFirst + mockLast,
                        Email = mockMail,
                        DateOfBirth = DateTime.Now.Date.AddYears(-30).Subtract(TimeSpan.FromDays(30 * i)),
                        FirstName = mockFirst,
                        LastName = mockLast,
                        PhoneNumber = mockNumber.RemoveNonNumeric(),
                        AccountType = AccountType.MockedData,
                        Gender = i % 2 == 0 ? AccountGender.Male : AccountGender.Female
                    };
                    var result = await _userManager.CreateAsync(user, mockPass);
                }
            }

            //update current location data for mock users
            /////////////////////////////////////////////
            double latmin = 30.3740;
            double latmax = 30.4251;
            double lonmin = -97.7501;
            double lonmax = -97.7001;
            Random randomLat = new Random((int)DateTime.Now.Ticks);
            Random randomLon = new Random((int)DateTime.Now.Ticks);
            Random randomMin = new Random((int)DateTime.Now.Ticks);
            var mockedUsers = await _context.ApplicationUsers
                                            .Where(a => a.AccountType.Equals(AccountType.MockedData))
                                            .ToListAsync();
            foreach (var user in mockedUsers)
            {
                DateTime timeStamp = DateTime.Now.Subtract(TimeSpan.FromMinutes(randomMin.NextDouble() * 60));
                double lat = randomLat.NextDouble() * (latmax - latmin) + latmin;
                double lon = randomLon.NextDouble() * (lonmax - lonmin) + lonmin;
                //user.CurrentTimeStamp = timeStamp;
                //user.CurrentLatitude = lat;
                //user.CurrentLongitude = lon;
                await _locationHistoryService.DeleteAllLocationHistoryAsync(user.Id);
                await _locationHistoryService.CreateLocationHistoryAsync(user.Id, lat, lon, timeStamp);
                _hexagonal.Initialize(lat, lon, _hexagonal.Layers[0]);
                String layers = _hexagonal.AllLayersDelimited();
                var currentLayer = await _context.CurrentLayers
                                                 .FirstOrDefaultAsync(c => c.UserId.Equals(user.Id));
                if (currentLayer == null)
                {
                    await _context.CurrentLayers.AddAsync(new CurrentLayer()
                    {
                        UserId = user.Id,
                        LayersDelimited = layers,
                        TimeStamp = timeStamp
                    });
                }
                else
                {
                    currentLayer.LayersDelimited = layers;
                    currentLayer.TimeStamp = timeStamp;
                }
            }
            await _context.SaveChangesAsync();

            //establish friend-requests for all of the mock users
            //////////////////////////////////////////
            var ryan = await _context.ApplicationUsers
                                     .FirstOrDefaultAsync(a => a.Email.Equals("rauch.ryan@gmail.com", StringComparison.OrdinalIgnoreCase));
            mockedUsers = await _context.ApplicationUsers
                                        .Where(a => a.AccountType.Equals(AccountType.MockedData))
                                        .ToListAsync();
            foreach (var initiator in mockedUsers)
            {
                DateTime timeStamp = DateTime.Now.Subtract(TimeSpan.FromMinutes(randomMin.NextDouble() * 60));
                var friendRequest = await _context.FriendRequests
                                                  .FirstOrDefaultAsync(f => f.InitiatorId.Equals(initiator.Id)
                                                                            && f.TargetId.Equals(ryan.Id));
                if (friendRequest == null)
                {
                    await _context.FriendRequests
                                  .AddAsync(new FriendRequest()
                                  {
                                      InitiatorId = initiator.Id,
                                      TargetId = ryan.Id,
                                      TimeStamp = DateTime.Now,
                                      Type = FriendRequestType.Normal
                                  });
                }
                else
                {
                    friendRequest.TimeStamp = timeStamp;
                }
            }
            await _context.SaveChangesAsync();
        }
    }
}
