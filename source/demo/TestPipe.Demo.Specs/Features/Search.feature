﻿@Demo @Search
Feature: Demo Search
	In order to find what I am looking for
	As a user
	I want to be able to search 

@Search @Manual
Scenario: 1. Search for Something
	Given I am on the search page
	And when I do a search
	When I submit the search
	Then results should be displayed

Scenario: 2. Search for Something
	Given I am on the search page
	And when I do a search
	When I submit the search
	Then results should be displayed
