﻿namespace Hastnama.Ekipchi.Common.Message
{
    public static class PersianErrorMessage
    {

        #region Common

        public const string BadRequestQuery = "ورودی های داده شده معتبر نیست";
        public const string InvalidPagingOption = "پیجینگ وارد شده معتبر نیست";


        #endregion
        #region User

        public const string InvalidUserCredential = "گذرواؤه یا واژه کاربری معتبر نمیباشد";
        public const string UserIsDeActive = "حساب کاربری مورد نظر غیر فعال میباشد";
        public const string UserNotFound = "کاربر مورد نظر پیدا نشد";
        public const string UserAlreadyExist = "کاربری با واژه کاربری داده شده در سیستم موجئد است";
        public const string UnAuthorized = "شما دسترسی استفاده از این سرویس را ندارید";
        public const string InvalidEmailAddress = "ایمیل نامعتبر است";
        public const string InvalidMobile = "شماره موبایل نامعتبر است";
        public const string InvalidNameOrFamily = "نام یا نام خانوادگی معتبر نیست";
        public const string InvalidUserRole = "نقش کاربر نامعتبر است";
        public const string InvalidUserId = "ایدی کاربر معتبر نمیباشد";
        public const string EmailAddressAlreadyExist = "آدرس ایمیل در سیستم موجود میباشد";
        public const string MobileAlreadyExist = "شماره موبایل در سیستم موجود میباشد";
        public const string UsernameAlreadyExist = "نام کاربری در سیستم موجود میباشد";

        #endregion

        #region City

        public const string CityNotFound = "شهر وارد شده معتبر نیست";


        #endregion

    }
}