Leading the Test Effort
=======================

Release Planning
------- 

Full delivery team meeting to get team on same page.

- Discuss goals and objectives
- Roles and responsibilities
- Schedule
- Ticket, priority, scope, requirements, acceptance criteria overview.

Sprint Analysis 
----------

- Review - Review tickets and associated requirements to determine if they are
    - Complete
    - Unambiguous
    - Not conflicting with other requirements
    - Scope and sized correctly
    - Shared definition of done (acceptance criteria)
    - Testable 
- Walk-through - Team walks through requirements and raise questions.
- Feedback - Questions are discussed with business and answers are shared with team.

Sprint Planning
----------------------

Plan the testing effort.

- Risk Assessment
- Scope - Define types of tests to run
    - Functional
    - Performance
    - Scalability
    - Security
    - Usability
    - Automated
    - Manual
- Estimate 
- Schedule
- Sign off

Test Setup
-------

- Test Assets - collect test assets in common place (e.g. ticket or file share)
- Test Environment - Prepare test environment.
- Test Data - create test data for each test scope.
- Train - Walk-through unfamiliar aspects.   

Test Execution
-------

- Spec - Write test charters and executable test specifications.
- Spec Review - Review specs to ensure they are within standards.
- Code - Code automated tests.
- Code Review - Review code to ensure it is within standards.
- Run - run manual and automated tests.
- Report - prepare test report.
- Report Review - Review test and bug report to ensure it is within standards.

Test Management
------

 - Monitor
    - Test environment
    - Automation (build, test, deploy)
    - Team effort and compare with planned estimate and schedule.
    - Coverage
 - Report
    - Status Report - Share current status of testing effort with team.
        - This can be a public dashboard  
            - Blockers - anything impeding the testing effort
            - Schedule - on, behind, ahead
            - Estimate - on, over, under
            - Tickets - current workflow (dev, test ready, tested, automated...)
            - Coverage
            - Risk
    - Result Report - Share test results with team.
        - This can be a public dashboard for  both manual and automated tests.    
        - On ticket complete share results with team.
            - One interesting way to do this would be with videos on a blog where the team can review and comment with questions and recommendations for aditional testing. This can be a simple screen cast with associated specs and possibly narration.  
     - Reject Report - compilation of bug reports and disposition.
        -  Developer
        -  Status - valid bug, requirement issue, deployment issue...
        -  Notes - explanation of bug follow-up, additional testing, mitigation: add to regression, improve requirements, train tester...
 - Sprint Review
    - Review Test Reports: Status, Results, Defects   

Team Adjustment
------

- Retrospective    
    - Discuss what went right to see if it can be amplified.
    - Discuss what went wrong and come up with solutions to make it better.
    - Make sure any decisions have a champion (some one to take charge of the action), clear expectations, and milestones (due dates).


Daily
-----

- Plan today's work
- Scrum
- Address issues from Scrum
- Review CD
- Review yesterday's work
    - Test Review - review new tests and test reports
    - Reject Review - review new rejects and defect reports
    - Code Review - review source control logs
- Update Reject Report
- Update Test Status Report
- Update Test Result Report

Sprint
------

- Plan next sprint
    - Create list of sprint acceptance tests with estimates.
    - Create automated tests for new features and defects in sprint.
        - Feature Spec: we start with a walk through of the feature/defect change to understand acceptance criteria. Then we design the feature/scenrios.
        - Spec Automation 
            - Review the feature spec and determine if we have the flow and page objects necessary to automate the test. 
            - Determine if we have the page control properties and action methods necessary to automate the test. 
            - Determine if we have the test data available to automate the test. 
            - Determine how difficult it is to create missing pieces and if it is worthwhile. Does it produces enough value, will it be hard to maintain.
            - Estimate how long to create automation.
        - Manual Test - we run through the acceptance tests manually. This gives us the information we need to verify the automation is sound. For acceptance tests that can not be automated, manual testing gives us the validation to approve a ticket.

- Quarterly
    - Trend Analysis
        - Reject Report
        - Test Status Report
        - Test Result Report  
    - Team Performance Review 

Hiring
------

- Can you use source control effectively?
- Can you solve algorithm-type problems?
- Can you program in more than one language or technology?
- Do you do something to increase your education or skills every day?
- Do you name things appropriately?
- Can you communicate your ideas effectively?
- Do you understand basic design patterns?
- Do you know how to debug effectively?
- Do you test your own code?
- Do you share your knowledge?
- Do you use the best tools for your job?
- Can you build an actual application?
- Have you written automated tests with an object oriented language for 2yrs or more?
- Can you create maintainable automated test architectures?
- Can you script maintainable and fast build, deploy, test pipelines?
- Can you manage continuous delivery server and agent infrastructure?