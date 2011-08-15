﻿using AutoMoq;
using Moq;

namespace Bennington.AdminAccounts.Specs
{
    public class with_automoqer
    {
        public static AutoMoqer mocker;

        public with_automoqer()
        {
            mocker = new AutoMoqer();
        }

        public static T Create<T>()
        {
            return mocker.Resolve<T>();
        }

        public static Mock<T> GetMock<T>() where T : class
        {
            return mocker.GetMock<T>();
        }
    }
}