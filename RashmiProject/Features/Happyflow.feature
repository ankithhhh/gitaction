Feature: Happyflow



@tag1
Scenario: SaugeLab
	Given User is on the Login Page
    When User enters the Username "<firstname>"
    And User enters the Password "<password>"
    And Clicks the Login button
    Then Homepage should open
    When User clicks on an Item
    Then Item details should be open
     Given User is logged in
    When User clicks on Add to Cart
    And User clicks on Cart
    Then User Cart should open
     Given User is on the Checkout page
    When User enters First Name "<Firstname>", Last Name "<lastname>", and Zip Code "<zipcode>"
    Then Clicks on Continue
     Given User is on the Checkout Overview page
    When User clicks on Finish
    Then Order status should be visible

   Examples: 
   | firstname     | password     |    Firstname | lastname | zipcode | 
    | standard_user | secret_sauc |    ankith    | amin    | 12345 |
    
