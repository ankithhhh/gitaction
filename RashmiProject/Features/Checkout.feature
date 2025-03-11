@Checkout
Feature:4Checkout

Scenario: User Address Details and Order
    Given User is on the Checkout page
    When User enters First Name "<Firstname>", Last Name "<lastname>", and Zip Code "<zipcode>"
    Then Clicks on Continue

    Examples: 
    | Firstname | lastname | zipcode |
    | sanath    | kumar    | 553909 |


