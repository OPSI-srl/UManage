Changelog
=========

**1.1.0** (*2016-04-07*)

Generic:

* Added a logging system.
* Moved UManage to DNN MVC architecture.
* Separation of concerns: module business logic located in ~/desktopmodules/UManage
  and module client located into ~/MVC/UManage/.

Backend:

* Fully rewritten backend part, implementing repository pattern.

Frontend:

* Added a records per page numeric input.
* Feature freeze before full rewriting.

Bugfix:

* Fixed a bug while exporting the user (the url for the export was hard coded).
* Fixed a bug which allowed user creation with non matching passwords.
* Fixes a glitch with modal dialogs
