using CardinalLibrary;
using CardinalLibrary.DataContracts;
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

        private const int NUMBEROFMOCKUSERS = 25;

        public async Task InitializeMockUsers(IServiceProvider serviceProvider, MockDataInitializeContract mdata = null)
        {
            _userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            _context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            _hexagonal = serviceProvider.GetRequiredService<IHexagonal>();
            _locationHistoryService = serviceProvider.GetRequiredService<ILocationHistoryService>();

            int i = 0;

            if (mdata == null)
            {
                mdata = new MockDataInitializeContract()
                {
                    Email = "rauch.ryan@gmail.com",
                    Latitude = 30.3986877,
                    Longitude = -97.72359399999999
                };
            }

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
                for (i = 1; i <= NUMBEROFMOCKUSERS; ++i)
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

            double latmin = mdata.Latitude - 0.01d;
            double latmax = mdata.Latitude + 0.01d;
            double lonmin = mdata.Longitude - 0.0025d;
            double lonmax = mdata.Longitude + 0.0025d;

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
                                     .FirstOrDefaultAsync(a => a.Email.Equals(mdata.Email, StringComparison.OrdinalIgnoreCase));
            if(ryan == null)
            {
                ryan = await _context.ApplicationUsers
                                     .FirstOrDefaultAsync(a => a.Email.Equals("rauch.ryan@gmail.com", StringComparison.OrdinalIgnoreCase));
            }
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


            ///////////////////////////////////////////////////
            // Zone and ZoneShape Data
            ///////////////////////////////////////////////////
            // West 6th
            string zoneName = "West 6th";
            var currentZone = await _context.Zones.FirstOrDefaultAsync(z => z.Description.Equals(zoneName, StringComparison.OrdinalIgnoreCase));
            if(currentZone == null)
            {
                currentZone = new Zone() { Description = zoneName, ARGBFill= "8095C6E4", Type=ZoneType.BarDistrict };
                await _context.Zones.AddAsync(currentZone);
                await _context.SaveChangesAsync();
            }
            currentZone = await _context.Zones.FirstOrDefaultAsync(z => z.Description.Equals(zoneName, StringComparison.OrdinalIgnoreCase));
            var currentZoneShapes = await _context.ZoneShapes.Where(z => z.ParentZoneId.Equals(currentZone.ZoneID)).ToArrayAsync();
            _context.ZoneShapes.RemoveRange(currentZoneShapes);
            await _context.SaveChangesAsync();
            i = 0;
            _context.ZoneShapes.Add(new ZoneShape() { ParentZone = currentZone, ParentZoneId = currentZone.ZoneID, Order = ++i, Latitude = 30.273021, Longitude = -97.749524 });
            _context.ZoneShapes.Add(new ZoneShape() { ParentZone = currentZone, ParentZoneId = currentZone.ZoneID, Order = ++i, Latitude = 30.271798, Longitude = -97.745204 });
            _context.ZoneShapes.Add(new ZoneShape() { ParentZone = currentZone, ParentZoneId = currentZone.ZoneID, Order = ++i, Latitude = 30.268091, Longitude = -97.746655 });
            _context.ZoneShapes.Add(new ZoneShape() { ParentZone = currentZone, ParentZoneId = currentZone.ZoneID, Order = ++i, Latitude = 30.269296, Longitude = -97.750918 });
            await _context.SaveChangesAsync();
            // Rockrose-domain
            zoneName = "Rockrose";
            currentZone = await _context.Zones.FirstOrDefaultAsync(z => z.Description.Equals(zoneName, StringComparison.OrdinalIgnoreCase));
            if (currentZone == null)
            {
                currentZone = new Zone() { Description = zoneName, ARGBFill="80FF0000", Type=ZoneType.BarDistrict };
                await _context.Zones.AddAsync(currentZone);
                await _context.SaveChangesAsync();
            }
            currentZone = await _context.Zones.FirstOrDefaultAsync(z => z.Description.Equals(zoneName, StringComparison.OrdinalIgnoreCase));
            currentZoneShapes = await _context.ZoneShapes.Where(z => z.ParentZoneId.Equals(currentZone.ZoneID)).ToArrayAsync();
            _context.ZoneShapes.RemoveRange(currentZoneShapes);
            await _context.SaveChangesAsync();
            i = 0;
            _context.ZoneShapes.Add(new ZoneShape() { ParentZone = currentZone, ParentZoneId = currentZone.ZoneID, Order = ++i, Latitude = 30.39983, Longitude = -97.723719 });
            _context.ZoneShapes.Add(new ZoneShape() { ParentZone = currentZone, ParentZoneId = currentZone.ZoneID, Order = ++i, Latitude = 30.40182, Longitude = -97.722989 });
            _context.ZoneShapes.Add(new ZoneShape() { ParentZone = currentZone, ParentZoneId = currentZone.ZoneID, Order = ++i, Latitude = 30.402172, Longitude = -97.724245 });
            _context.ZoneShapes.Add(new ZoneShape() { ParentZone = currentZone, ParentZoneId = currentZone.ZoneID, Order = ++i, Latitude = 30.403236, Longitude = -97.72374 });
            _context.ZoneShapes.Add(new ZoneShape() { ParentZone = currentZone, ParentZoneId = currentZone.ZoneID, Order = ++i, Latitude = 30.402606, Longitude = -97.721659 });
            _context.ZoneShapes.Add(new ZoneShape() { ParentZone = currentZone, ParentZoneId = currentZone.ZoneID, Order = ++i, Latitude = 30.399562, Longitude = -97.723011 });
            await _context.SaveChangesAsync();
            // Warehouse District
            zoneName = "Warehouse District";
            currentZone = await _context.Zones.FirstOrDefaultAsync(z => z.Description.Equals(zoneName, StringComparison.OrdinalIgnoreCase));
            if (currentZone == null)
            {
                currentZone = new Zone() { Description = zoneName, ARGBFill = "80D2B7D8", Type = ZoneType.BarDistrict };
                await _context.Zones.AddAsync(currentZone);
                await _context.SaveChangesAsync();
            }
            currentZone = await _context.Zones.FirstOrDefaultAsync(z => z.Description.Equals(zoneName, StringComparison.OrdinalIgnoreCase));
            currentZoneShapes = await _context.ZoneShapes.Where(z => z.ParentZoneId.Equals(currentZone.ZoneID)).ToArrayAsync();
            _context.ZoneShapes.RemoveRange(currentZoneShapes);
            await _context.SaveChangesAsync();
            i = 0;
            _context.ZoneShapes.Add(new ZoneShape() { ParentZone = currentZone, ParentZoneId = currentZone.ZoneID, Order = ++i, Latitude = 30.269036, Longitude = -97.74634 });
            _context.ZoneShapes.Add(new ZoneShape() { ParentZone = currentZone, ParentZoneId = currentZone.ZoneID, Order = ++i, Latitude = 30.268019, Longitude = -97.742779 });
            _context.ZoneShapes.Add(new ZoneShape() { ParentZone = currentZone, ParentZoneId = currentZone.ZoneID, Order = ++i, Latitude = 30.26522, Longitude = -97.743823 });
            _context.ZoneShapes.Add(new ZoneShape() { ParentZone = currentZone, ParentZoneId = currentZone.ZoneID, Order = ++i, Latitude = 30.266814, Longitude = -97.749481 });
            _context.ZoneShapes.Add(new ZoneShape() { ParentZone = currentZone, ParentZoneId = currentZone.ZoneID, Order = ++i, Latitude = 30.269279, Longitude = -97.750911 });
            _context.ZoneShapes.Add(new ZoneShape() { ParentZone = currentZone, ParentZoneId = currentZone.ZoneID, Order = ++i, Latitude = 30.268091, Longitude = -97.746655 });
            await _context.SaveChangesAsync();
            // 2nd Street
            zoneName = "2nd Street";
            currentZone = await _context.Zones.FirstOrDefaultAsync(z => z.Description.Equals(zoneName, StringComparison.OrdinalIgnoreCase));
            if (currentZone == null)
            {
                currentZone = new Zone() { Description = zoneName, ARGBFill = "806F7FBD", Type = ZoneType.BarDistrict };
                await _context.Zones.AddAsync(currentZone);
                await _context.SaveChangesAsync();
            }
            currentZone = await _context.Zones.FirstOrDefaultAsync(z => z.Description.Equals(zoneName, StringComparison.OrdinalIgnoreCase));
            currentZoneShapes = await _context.ZoneShapes.Where(z => z.ParentZoneId.Equals(currentZone.ZoneID)).ToArrayAsync();
            _context.ZoneShapes.RemoveRange(currentZoneShapes);
            await _context.SaveChangesAsync();
            i = 0;
            _context.ZoneShapes.Add(new ZoneShape() { ParentZone = currentZone, ParentZoneId = currentZone.ZoneID, Order = ++i, Latitude = 30.266517, Longitude = -97.748421 });
            _context.ZoneShapes.Add(new ZoneShape() { ParentZone = currentZone, ParentZoneId = currentZone.ZoneID, Order = ++i, Latitude = 30.26522, Longitude = -97.743823 });
            _context.ZoneShapes.Add(new ZoneShape() { ParentZone = currentZone, ParentZoneId = currentZone.ZoneID, Order = ++i, Latitude = 30.263367, Longitude = -97.744544 });
            _context.ZoneShapes.Add(new ZoneShape() { ParentZone = currentZone, ParentZoneId = currentZone.ZoneID, Order = ++i, Latitude = 30.264683, Longitude = -97.749128 });
            await _context.SaveChangesAsync();

            ///////////////////////////////////////////////////
            // FriendGroup data
            ///////////////////////////////////////////////////


        }
    }
}
