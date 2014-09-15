@Demo @Search
Feature: 1. Demo Search
	In order to find what I am looking for
	As a user
	I want to be able to search 

@Search
Scenario: 1. Search for Something
	Given I am on the search page
	And when I do a search for "TestPipe"
	When I submit the search
	Then results should be displayed
