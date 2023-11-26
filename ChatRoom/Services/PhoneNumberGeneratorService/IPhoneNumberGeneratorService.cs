using System.Text;

namespace ChatRoom.Services.PhoneNumberGeneratorService
{
    public interface IPhoneNumberGeneratorService
    {
   

    }

    public static class PhoneNumberGenerator
    {
        public static string GenerateRandomPhoneNumber()
        {
            return string.Format("09{0}", GeneratRandomNumber(8));
        }
        /// <summary>
        /// Create a random number as a string with a maximum length.
        /// </summary>
        /// <param name="length">Length of number</param>
        /// <returns>Generated string</returns>
        public static string GeneratRandomNumber(int length)
        {
            if (length > 0)
            {
                var sb = new StringBuilder();

                var rnd = SeedRandom();
                for (int i = 0; i < length; i++)
                {
                    sb.Append(rnd.Next(0, 9).ToString());
                }

                return sb.ToString();
            }

            return string.Empty;
        }
        private static Random SeedRandom()
        {
            return new Random(Guid.NewGuid().GetHashCode());
        }
    }

    public static class UserNameGenerator
    {
        private static string alphabet = "abcdefgijkmnopqrstuvwxyz";
        private static string shorturl_chars_ucase =alphabet.Reverse().ToString() ;
        public static string GenerateRandomUserName(int count =8)
        {
            return string.Format("{0}", GeneratRandomString(count));
        }
        /// <summary>
        /// Create a random number as a string with a maximum length.
        /// </summary>
        /// <param name="length">Length of number</param>
        /// <returns>Generated string</returns>
        public static string GeneratRandomString(int length)
        {
            var full =( alphabet + shorturl_chars_ucase).ToArray();
            if (length > 0)
            {
                var sb = new StringBuilder();

                var rnd = SeedRandom();
                for (int i = 0; i < length; i++)
                {
                   string text= full[rnd.Next(0, full.Length)].ToString();
                    sb.Append(text);
                }

                return sb.ToString();
            }

            return string.Empty;
        }
        private static Random SeedRandom()
        {
            return new Random(Guid.NewGuid().GetHashCode());
        }
    }
    public static class TextRandomGenerator
    {
		
		public static string Generate()
        {
            string text  =
                "لورم ایپسوم یا طرح‌نما (به انگلیسی: Lorem ipsum) به متنی آزمایشی و بی‌معنی در صنعت چاپ، صفحه‌آرایی و طراحی گرافیک گفته می‌شود." +
                "طراح گرافیک از این متن به عنوان عنصری از ترکیب بندی برای پر کردن صفحه و ارایه اولیه شکل ظاهری و کلی طرح سفارش گرفته شده استفاده می نماید، تا از نظر گرافیکی نشانگر چگونگی نوع و اندازه فونت و ظاهر متن باشد. " +
                " معمولا طراحان گرافیک برای صفحه‌آرایی، نخست از متن‌های آزمایشی و بی‌معنی استفاده می‌کنند تا صرفا به مشتری یا صاحب کار خود نشان دهند که صفحه طراحی یا صفحه بندی شده بعد از اینکه متن در آن قرار گیرد چگونه به نظر می‌رسد و قلم‌ها و اندازه‌بندی‌ها چگونه در نظر گرفته شده‌است. " +
                "از آنجایی که طراحان عموما نویسنده متن نیستند و وظیفه رعایت حق تکثیر متون را ندارند و در همان حال کار آنها به نوعی وابسته به متن می‌باشد آنها با استفاده از محتویات ساختگی، صفحه گرافیکی خود را صفحه‌آرایی می‌کنند تا مرحله طراحی و صفحه‌بندی را به پایان برند." +
                "ابتدا تعداد لورم ایپسوم های مورد نظر را مشخص کنید." +
                "نوع عباراتی که می توانند ساخته شوند را مشخص کنید یعنی انتخاب کنید که می خواهید چند کلمه ساخته شود یا جند جمله یا چند پاراگراف." +
                "بان مورد نظر را انتخاب کنید یعنی عبارات فارسی ساخته شوند یا عبارات انگلیسی." +
                "در انتها با استفاده از دکمه \"تولید عبارت تصادفی\"، لورم ایپسوم های خود را مشاهده کنید."
                + "شهر دهدشت هویت و خاطره تاریخی خود را از مجموعه بلادشاپور (بافت قدیم شهر) می‌گیرد. این مجموعه که در گذشته مطمئن‌ترین و نزدیک‌ترین مرکز ارتباط تجاری بین اصفهان (مرکز ایران) و بنادر جنوبی کشور بوده‌است در همسو نگری و همگرایی اقوام و شهروندان شهر دهدشت نقش بسزایی داشته و دارد.\r\nعلاوه بر این دهدشت محل عبور سپاهیان اسکندر مقدونی در حمله به تخت جمشید بوده‌است. تنگ‌تکاب در نزدیکی شهر دهدشت که محل درگیری سپاهیان آریوبرزن با اسکندر مقدونی بوده‌است؛ گذرگاهی بسیار مهمی تلقی می‌شد . چرا که مسیر اصلی دو استان باستانی ایران، یعنی فارس و خوزستان بود. مسیر عبور شوش به پارسه از ارجان (بهبهان) به بلادشاپور (دهدشت) و سپس به تخت جمشید بوده؛ به همین خاطر یونانی‌ها به تنگ تکاب می‌گفتند. شاید کمتر مسیر ترددی در ایران بتوان پیدا کرد که به اندازهٔ این تنگه مورد اشارهٔ تاریخ نگاران و باستان‌شناس‌ها قرار گرفته باشد.\r\nدهدشت علیرغم ثبت ملی شدن در سال ۶۴ و مورد بازدید قرار گرفتن توسط یونسکو در سال ۱۴۰۰، پس از دوره صفویه هیچ‌گاه مورد توجه و اقدام جدی جهت بازسازی، مرمت یا حتی نگهداری قرار نگرفت و همواره مورد بی مهری واقع شد که بخش عمده ای از این بی مهری و کم لطفی از عدم آگاهی دوگانه مردم مسئولان از پتانسیل‌های اجتماعی فرهنگی این مجموعه ناشی می‌شود بلادشاپور که باور برخی پیشینهٔ آن به دورهٔ ساسانیان هم می‌رسد یک هویت تاریخی زنده اما غیر پویا است که می‌توان با توجه نقش هویت ساز آنچه به لحاظ اقتصادی و چه به لحاظ فرهنگی اجتماعی آورده‌ها و منافع زیادی را نصیب شهروندان کرد.\r\n\r\n"
                + "این شهر در دوره ساسانیان توسط پسر اردشیر اول ساخته شده است. و در آن دوران به نام بلاد شاپور شهرت داشت.در دوره‌های پسین نیز رونق داشت اما شروع پی ریزی شهر کنونی دهدشت را پس از دوره مغولان و به عبارتی از اواخر تیموریان و پس از آن جستجو می‌کنند. ولی بافت قدیم دهدشت را که بخش‌هایی از آن بر جای مانده‌است، منسوب به دوره صفوی می‌دانند. بافت قدیم دهدشت که ۳۵هکتار وسعت دارد در سال ۱۳۶۴در فهرست آثار ملی کشور به ثبت رسیده‌است . "
				;
            var arrayString = text.Split(".");
			 
			  
			return  arrayString[SeedRandom().Next(arrayString.Length)];
		}
		private static Random SeedRandom()
		{
			return new Random(Guid.NewGuid().GetHashCode());
		}

        public static string RandomName()
        {
            var names = "    پرخیده  , ارشا  , ایرانمهر  , اسپنوی  , گرایش  , رادنوش , داراب  , اشتاد  , ایران پناه  , تَندُر  , افرینا  , دارا  , هوشنگ  , آناهیتا , پوریا  , دادفر  , گل آذین  , آزادچهر  , گوهربانو  , خُوروَش  , گلنواز  , به آفرید  , آفرینمهر  , آریامن ,    شیدبانو  ,       گلبهار  ,       مژگان  ,       آذرگل  ,       زادمهر  ,       هورزاد  ,       گشتاسب  ,       آریا ناز ,       جانمهر  ,       نوید  ,       فیروزه  ,       هورسان  ,       نسیم  ,       آویده  ,       پریمرز  ,       بَرازمان  ,       آرمیتا  ,       ارشین ,       آوا  ,       گلزاد  ,       آذرچهر  ,       اَوَرکام  ,       آلاله  ,       نیوتیش  ".Split(',');
            return names[Utility.RandomKey.GetRandomNumber(0, names.Length - 1)];
		}
	}


}
