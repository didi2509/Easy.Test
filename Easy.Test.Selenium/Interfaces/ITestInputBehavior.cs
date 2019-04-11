﻿using OpenQA.Selenium;
using System;
using Easy.Test.Selenium.Interfaces;

namespace Easy.Test.SeleniumTest.Interfaces
{
    public interface ITestInputBehavior<TInput>
    {
        TInput GetTestInput();
        TInput GetDefaultTestInput();
    }
}
