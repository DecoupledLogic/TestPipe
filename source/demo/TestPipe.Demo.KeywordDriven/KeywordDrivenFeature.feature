Feature: KeywordDrivenFeature

Scenario: Enter Welcome Text
	Given I am on the "Welcome" page
	And I enter "Hello" in "Welcome"
	When I click "Submit"
	Then I should be on the "Success" page
	And "Header" text should be "Success"

Scenario: Enter Welcome Text By Data Table
	Given I am on the page under test
	And I enter "Welcome"
	When I click "Submit"
	Then I should be on the result page
	And "Header" text should be "Success"

	#Keywords
	#Data
		#Input
		#Assert