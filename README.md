# MKopaInterview
Broadly speaking, your design will focus on M-KOPA’s “loan management” system. As an
institution that sells solar devices to customers on credit, M-KOPA manages a portfolio of
loans for customers. This exercise covers three basic operations:
1. Creating a new customer record
2. Adding a new loan to a customer’s account
3. Accepting a payment for a particular loan

### Creating a New Customer Record

Customer registration requests can be assumed to arrive via HTTP POST from a point-of-
sale (PoS) application; the exact details of PoS application can be considered out-of-scope

for this exercise. Each new customer record contains the following fields:
- firstName: customer’s first name
- lastName: customer’s family name
- dateOfBirth: customer’s date of bith
- idNumber: customer’s national ID number
- phoneNumber: customer’s cell phone number
