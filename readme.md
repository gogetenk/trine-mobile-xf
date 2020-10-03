# Trine Mobile
An app that makes the consultants activity reports filling cool, easy, and fast.

[![Build status](https://build.appcenter.ms/v0.1/apps/e5d75035-be14-4426-8c1c-1c93d1ad4cb7/branches/master/badge)](https://appcenter.ms)
[![Build status](https://build.appcenter.ms/v0.1/apps/94499a8f-dde8-45a0-b376-74cff0282cb7/branches/dev/badge)](https://appcenter.ms)

## Goal
See our landing page at https://hellotrine.com if you provide your consultants to companies and at https://hellotrine.com/fr/customers if you have consultants working for your company.

I have open sourced this app because I've stopped its development, and replaced it by a **React Native version** (sorry MS).

## Project Architecture
This is a ***Xamarin.Forms*** project with a n-tiers architecture :
- model layer
- business logic layer
- data access layer
- presentation layer (here, a bootstrapper and 3 native mobile projects)

Each layer has its own abstraction layer in order to avoid dependencies. 
I'm using the ***Prism (with Autofac)*** IOC and DI engine.


### Modules
The application is splitted in Prism Modules in order to enhance performances (lazy loading) and enforce loosely coupled components.
There is one common module that is loaded at launch (authentication module), and three modules that are loaded depending on the user type (Consultant, Commercial or Customer).

Using the modular approach, I save loading time and memory by not loading UIs and Services the user won't use.
Ex : the commercial or the customer will absolutely never create an activity report. 

##### And for cross-cutting features ?
They are shared accross modules using netstandard 2.0 libraries.

#### Authentication Module (loaded on start)
![](https://i.imgur.com/wRLOtLp.png)

#### Consultant Module (lazy loaded)
![](https://i.imgur.com/1PukEbh.png)

#### Customer Module (lazy loaded)
![](https://i.imgur.com/RyY0FCS.png)

#### Commercial Module (lazy loaded)
![](https://i.imgur.com/waDkgPQ.png)


### Tests
#### Unit Tests
Each services (BLL) and viewmodels are unit tested using **xUnit**. Every native mobile library I use are behind an abstraction layer, so they can all be mocked.
#### UI Tests
I'm using **Xamarin.UITests** library, that is using **nUnit** and Nespresso engine behind the scenes. 
The **Page Object pattern** is used in order to facilitate the UI tests.
Since those are **extremely** slow to execute in the CI/CD, I only test critical user paths, only when releasing production binaries.

![](https://i.imgur.com/UkriRTk.png)

## Logging 
I'm using **sentry.io** for exception and crash logging, and **Azure Application Insights** for functional logs.

## Customer support and documentation
I've choosen **Intercom** service for customer support.
So I'm using the Intercom SDK bindings for Xamarin.Forms in order to use directly the official native SDK for iOS and Android.

## Bug tracking and QA
I'm using **Instabug** so my users and I can just shake the device in order to report a bug, with additional cool features like screenshots, annotations, attachments, logs, view structure reports etc...

## Delivery
![](https://i.imgur.com/AkXw1aT.png)
I'm using **appcenter.ms** in order to build, test, release and monitor this application on all platforms.

