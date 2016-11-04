using Teaching.Skills.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using PCLStorage;

namespace Teaching.Skills.Contexts
{
    public sealed class DefaultContext
    {

        #region Singleton

        private static readonly DefaultContext instance = new DefaultContext();

        private DefaultContext()
        {

            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                TypeNameHandling = TypeNameHandling.Objects,
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

        }

        public static DefaultContext Instance
        {
            get
            {
                return instance;
            }
        }

        #endregion

        public async Task LoadAsync(Stream inputSource)
        {

            try
            {

                //Stream inputSource = Android.App.Application.Context.Assets.Open("DataSource.json");
                using (StreamReader reader = new StreamReader(inputSource))
                {
                    var json = reader.ReadToEnd();
                    var list = JsonConvert.DeserializeObject<IEnumerable<Category>>(json);
                    Categories = new ObservableCollection<Category>(list);

                    var indicators = from item in Categories.SelectMany(i => i.Indicators) select item;
                    Indicators = new ObservableCollection<Indicator>(indicators);

                    var questions = from item in Indicators.SelectMany(i => i.Questions) select item;
                    Questions = new ObservableCollection<Question>(questions);

                }

                Users = new ObservableCollection<User>();

                IFolder rootFolder = FileSystem.Current.LocalStorage;
                string path = rootFolder.Path;
                string filename = Path.Combine(path, "Cache.json");
                var exist = await FileSystem.Current.LocalStorage.CheckExistsAsync(filename);

                if (exist == ExistenceCheckResult.FileExists)
                {
                    var data = await FileSystem.Current.GetFileFromPathAsync(filename);
                    if (data != null)
                    {

                        var json = await data.ReadAllTextAsync();
#if DEBUG
                        System.Diagnostics.Debug.WriteLine(json);
#endif
                        if (!string.IsNullOrEmpty(json))
                        {
                            var list = JsonConvert.DeserializeObject<IEnumerable<User>>(json);
                            Users = new ObservableCollection<User>(list);
                        }

                    }

                }

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

        }

        public ObservableCollection<Category> Categories { get; set; } = new ObservableCollection<Category>();
        public ObservableCollection<Indicator> Indicators { get; set; } = new ObservableCollection<Indicator>();
        public ObservableCollection<Question> Questions { get; set; } = new ObservableCollection<Question>();
        public ObservableCollection<User> Users { get; set; } = new ObservableCollection<User>();

        public async Task SaveAsync()
        {
            try
            {
                var json = JsonConvert.SerializeObject(Users);
                IFolder rootFolder = FileSystem.Current.LocalStorage;
                string path = rootFolder.Path;

                string filename = Path.Combine(path, "Cache.json");

                var file = await FileSystem.Current.GetFileFromPathAsync(filename);
                await file.WriteAllTextAsync(json);

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

        }

    }
}

