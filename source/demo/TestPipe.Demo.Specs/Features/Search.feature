@Demo @Search
Feature: Demo Search
	In order to find what I am looking for
	As a user
	I want to be able to search 

@Demo @Search @Smoke
Scenario: 1. Search for Something
	Given I am on the search page
	When I submit a search
	Then results should be displayed

@Demo @Search
Scenario: 2. Search for Something
	Given I am on the search page
	When I submit a search
	Then results should be displayed
