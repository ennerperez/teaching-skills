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
using Teaching.Skills.Core;

namespace Teaching.Skills.Contexts
{
	public sealed class DefaultContext
	{

#if DEBUG
		internal readonly bool Clear = false;
#endif

		internal const string DataSource = "cache.json";

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

		public ObservableCollection<Category> Categories { get; set; } = new ObservableCollection<Category>();
		public ObservableCollection<Indicator> Indicators { get; set; } = new ObservableCollection<Indicator>();
		public ObservableCollection<Question> Questions { get; set; } = new ObservableCollection<Question>();
		public ObservableCollection<User> Users { get; set; } = new ObservableCollection<User>();

		public async Task LoadAsync(Stream inputSource)
		{

#if DEBUG
			if (Clear)
				await ClearAsync();
#endif

			try
			{

				using (StreamReader reader = new StreamReader(inputSource))
				{
					var json = reader.ReadToEnd();
					var pack = JsonConvert.DeserializeObject<Pack>(json);

					var categories = pack.Content;
					Categories = new ObservableCollection<Category>(categories);

					var indicators = from item in Categories.SelectMany(i => i.Indicators) select item;
					Indicators = new ObservableCollection<Indicator>(indicators);

					var questions = from item in Indicators.SelectMany(i => i.Questions) select item;
					Questions = new ObservableCollection<Question>(questions);

				}

				Users = new ObservableCollection<User>();

				IFolder rootFolder = FileSystem.Current.LocalStorage;

				string fileName = DataSource;
				var exist = await rootFolder.CheckExistsAsync(fileName);

				IFile file = null;
				if (exist == ExistenceCheckResult.FileExists)
				{
					file = await rootFolder.GetFileAsync(fileName);
					if (file != null)
					{
						var json = await file.ReadAllTextAsync();
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
				else
					throw new FileNotFoundException();

			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.Message);
			}
		}
		public async Task SaveAsync()
		{
			try
			{
				var json = JsonConvert.SerializeObject(Users);
				IFolder rootFolder = FileSystem.Current.LocalStorage;

				string fileName = DataSource;
				var exist = await rootFolder.CheckExistsAsync(fileName);

				IFile file = null;
				if (exist == ExistenceCheckResult.NotFound)
					file = await rootFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
				else
					file = await rootFolder.GetFileAsync(fileName);

				await file.WriteAllTextAsync(json);

				if (file == null)
					throw new FileNotFoundException();

			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.Message);
			}

		}
		public async Task ClearAsync()
		{
			try
			{

				IFolder rootFolder = FileSystem.Current.LocalStorage;

				string fileName = DataSource;
				var exist = await rootFolder.CheckExistsAsync(fileName);
				if (exist == ExistenceCheckResult.FileExists)
				{
					var file = await rootFolder.GetFileAsync(fileName);
					await file.DeleteAsync();
				}

			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.Message);
			}
		}

	}
}

