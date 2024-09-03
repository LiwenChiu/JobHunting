USE Duck
GO
INSERT INTO TitleCategories (TitleCategoryID, TitleCategoryName) VALUES
('A', N'農、林、漁、牧業'),
('B', N'礦業及土石採取業'),
('C', N'製造業'),
('D', N'水電燃氣業'),
('E', N'營造及工程業'),
('F', N'批發、零售及餐飲業'),
('G', N'運輸、倉儲及通信業'),
('H', N'金融、保險及不動產業'),
('I', N'專業、科學及技術服務業'),
('J', N'文化、運動、休閒及其他服務業'),
('Z', N'其他未分類業');
GO
INSERT INTO TitleClasses (TitleClassID, TitleCategoryID, TitleClassName) VALUES
('A1', 'A', N'農業'),
('A2', 'A', N'林業及伐木業'),
('A3', 'A', N'漁業'),
('A4', 'A', N'牧業'),
('B1', 'B', N'能源礦業'),
('B2', 'B', N'其他礦業'),
('B6', 'B', N'土石採取業'),
('C1', 'C', N'食品、飲料及菸類製造業'),
('C2', 'C', N'飼料及寵物食品製造業'),
('C3', 'C', N'紡織、成衣、服飾品及紡織製品製造業'),
('C4', 'C', N'皮革、毛皮及其製品製造業'),
('C5', 'C', N'木竹製品製造業'),
('C6', 'C', N'紙漿、紙及紙製品業'),
('C7', 'C', N'印刷及其輔助業'),
('C8', 'C', N'化學業'),
('C9', 'C', N'非金屬礦物製品製造業'),
('CA', 'C', N'金屬業'),
('D1', 'D', N'電力供應業'),
('D2', 'D', N'氣體燃料供應業'),
('D3', 'D', N'自來水供應業'),
('D4', 'D', N'熱能供應業'),
('D5', 'D', N'溫泉取供業'),
('D6', 'D', N'再生水經營業'),
('E1', 'E', N'營造業'),
('E4', 'E', N'海事工程業'),
('E5', 'E', N'管道工程業'),
('E6', 'E', N'機電工程業'),
('E7', 'E', N'電信工程業'),
('E8', 'E', N'建物裝修及裝潢業'),
('E9', 'E', N'油漆及防蝕工程業'),
('EZ', 'E', N'其他工程業'),
('F1', 'F', N'批發業'),
('F2', 'F', N'零售業'),
('F3', 'F', N'綜合零售業'),
('F4', 'F', N'國際貿易業'),
('F5', 'F', N'餐飲業'),
('F6', 'F', N'智慧財產權業'),
('G1', 'G', N'陸上運輸業'),
('G2', 'G', N'陸上運輸輔助業'),
('G3', 'G', N'水上運輸業'),
('G4', 'G', N'水上運輸輔助業'),
('G5', 'G', N'航空運輸業'),
('G6', 'G', N'航空運輸輔助業'),
('G7', 'G', N'其他運輸輔助業'),
('G8', 'G', N'倉儲業'),
('G9', 'G', N'電信業'),
('GA', 'G', N'郵政業'),
('H1', 'H', N'金融業'),
('H2', 'H', N'投資典當業'),
('H3', 'H', N'證券業'),
('H4', 'H', N'期貨業'),
('H5', 'H', N'保險業'),
('H6', 'H', N'保險輔助人'),
('H7', 'H', N'不動產業'),
('H8', 'H', N'金融控股公司業'),
('H9', 'H', N'證券期貨控股業'),
('HZ', 'H', N'其他金融、保險及不動產業'),
('I1', 'I', N'顧問服務業'),
('I3', 'I', N'資訊服務業'),
('I4', 'I', N'廣告業'),
('I5', 'I', N'設計業'),
('I7', 'I', N'就業服務業'),
('I8', 'I', N'公寓大廈管理服務業'),
('I9', 'I', N'保全業'),
('IA', 'I', N'工業檢驗業'),
('IB', 'I', N'建築物公共安全檢查業'),
('IC', 'I', N'藥品檢驗業'),
('J1', 'J', N'環保服務業'),
('J2', 'J', N'訓練服務業'),
('J3', 'J', N'出版事業'),
('J4', 'J', N'電影事業'),
('J5', 'J', N'廣播電視業'),
('J6', 'J', N'藝文業'),
('J7', 'J', N'休閒、娛樂服務業'),
('J8', 'J', N'運動服務業'),
('J9', 'J', N'觀光及旅遊服務業'),
('JA', 'J', N'個人服務業'),
('ZZ', 'Z', N'其他未分類業');
GO
INSERT INTO Companies (GUINumber, [Password], CompanyName, TitleClassID, [Address], Intro, Benefits, ContactName, ContactPhone, ContactEmail, [Status], [Date]) VALUES
(10000001, 'comp1pass', 'Tech Solutions', 'B1', '123 Main St', 'Leading technology solutions provider', 'Health insurance, 401k', 'Alice Johnson', '555-1234', 'alice@techsolutions.com', 1, GETDATE()),
(10000002, 'comp2pass', 'Green Energy', 'B2', '456 Elm St', 'Renewable energy company', 'Stock options, Flexible hours', 'Bob Smith', '555-5678', 'bob@greenenergy.com', 1, GETDATE()),
(10000003, 'comp3pass', 'Creative Designs', 'J1', '789 Oak St', 'Innovative design firm', 'Paid leave, Health insurance', 'Charlie Brown', '555-8765', 'charlie@creativedesigns.com', 1, GETDATE()),
(10000004, 'comp4pass', 'Healthcare Plus', 'A2', '321 Pine St', 'Healthcare services provider', 'Health insurance, Dental plan', 'Dana White', '555-4321', 'dana@healthcareplus.com', 1, GETDATE()),
(10000005, 'comp5pass', 'Finance World', 'H1', '654 Maple St', 'Financial consulting and services', '401k, Paid holidays', 'Evan Green', '555-6543', 'evan@financeworld.com', 1, GETDATE()),
(10000006, 'comp6pass', 'Tech Innovators', 'G1', '987 Cedar St', 'Innovative technology solutions', 'Gym membership, Health insurance', 'Fiona Blue', '555-7890', 'fiona@techinnovators.com', 1, GETDATE()),
(10000007, 'comp7pass', 'Legal Eagle', 'I1', '147 Birch St', 'Legal services and consulting', 'Health insurance, Stock options', 'George Fox', '555-8523', 'george@legaleagle.com', 1, GETDATE()),
(10000008, 'comp8pass', 'Sales Empire', 'IA', '258 Spruce St', 'Leading sales and distribution', 'Commission, Health insurance', 'Helen King', '555-9630', 'helen@salesempire.com', 1, GETDATE()),
(10000009, 'comp9pass', 'Marketing Magic', 'D1', '369 Fir St', 'Creative marketing solutions', 'Flexible hours, Stock options', 'Ivy Green', '555-7410', 'ivy@marketingmagic.com', 1, GETDATE()),
(10000010, 'comp10pass', 'Customer First', 'F1', '852 Ash St', 'Customer service and support', 'Paid leave, Health insurance', 'Jack Black', '555-7531', 'jack@customerfirst.com', 1, GETDATE());
GO
INSERT INTO Openings (CompanyID, Title, TitleClassID, [Address], [Description], Benefits, InterviewYN, SalaryMax, SalaryMin, [Time], ContactName, ContactPhone, ContactEmail, ReleaseYN) VALUES
(1, 'Senior Software Engineer', 'B1', '123 Main St', 'Develop and maintain software applications', 'Health insurance, Stock options', 1, 120000, 80000, 'Full-time', 'Alice Johnson', '555-1234', 'alice@techsolutions.com', 1),
(2, 'Renewable Energy Consultant', 'B2', '456 Elm St', 'Consult on renewable energy projects', 'Stock options, Flexible hours', 1, 90000, 60000, 'Full-time', 'Bob Smith', '555-5678', 'bob@greenenergy.com', 1),
(3, 'Graphic Designer', 'J1', '789 Oak St', 'Create visual content for marketing campaigns', 'Paid leave, Health insurance', 1, 70000, 50000, 'Full-time', 'Charlie Brown', '555-8765', 'charlie@creativedesigns.com', 1),
(4, 'Healthcare Administrator', 'A2', '321 Pine St', 'Manage healthcare operations and staff', 'Health insurance, Dental plan', 1, 95000, 70000, 'Full-time', 'Dana White', '555-4321', 'dana@healthcareplus.com', 1),
(5, 'Financial Analyst', 'H1', '654 Maple St', 'Analyze financial data and provide insights', '401k, Paid holidays', 1, 85000, 60000, 'Full-time', 'Evan Green', '555-6543', 'evan@financeworld.com', 1),
(6, 'Junior IT Support Specialist', 'G1', '987 Cedar St', 'Provide IT support and troubleshooting', 'Gym membership, Health insurance', 1, 55000, 40000, 'Full-time', 'Fiona Blue', '555-7890', 'fiona@techinnovators.com', 1),
(7, 'Paralegal', 'I1', '147 Birch St', 'Assist lawyers in legal research and case preparation', 'Health insurance, Stock options', 1, 60000, 45000, 'Full-time', 'George Fox', '555-8523', 'george@legaleagle.com', 1),
(8, 'Sales Executive', 'IA', '258 Spruce St', 'Drive sales and build client relationships', 'Commission, Health insurance', 1, 80000, 50000, 'Full-time', 'Helen King', '555-9630', 'helen@salesempire.com', 1),
(9, 'Digital Marketing Specialist', 'D1', '369 Fir St', 'Manage digital marketing campaigns', 'Flexible hours, Stock options', 1, 75000, 50000, 'Full-time', 'Ivy Green', '555-7410', 'ivy@marketingmagic.com', 1),
(10, 'Customer Support Agent', 'F1', '852 Ash St', 'Provide customer service and support', 'Paid leave, Health insurance', 1, 50000, 35000, 'Full-time', 'Jack Black', '555-7531', 'jack@customerfirst.com', 1);
GO
INSERT INTO Candidates (NationalID, Email, [Password], [Name], Sex, Birthday, Phone, [Address], Degree, EmploymentStatus, MilitaryService) VALUES
('A123456789', 'alice@example.com', 'pass123', 'Alice Chen', 0, '1990-01-01', '555-1111', '101 Rose Ave', 'Bachelor', 'Employed', 'Completed'),
('B987654321', 'bob@example.com', 'pass456', 'Bob Lin', 1, '1985-02-02', '555-2222', '202 Tulip Ave', 'Master', 'Unemployed', 'Completed'),
('C567890123', 'charlie@example.com', 'pass789', 'Charlie Wong', 1, '1992-03-03', '555-3333', '303 Orchid Ave', 'Bachelor', 'Employed', 'Exempt'),
('D234567890', 'dana@example.com', 'pass101', 'Dana Lee', 0, '1988-04-04', '555-4444', '404 Lily Ave', 'PhD', 'Self-Employed', 'Exempt'),
('E345678901', 'evan@example.com', 'pass202', 'Evan Tan', 1, '1991-05-05', '555-5555', '505 Daisy Ave', 'Master', 'Employed', 'Completed'),
('F456789012', 'fiona@example.com', 'pass303', 'Fiona Huang', 0, '1993-06-06', '555-6666', '606 Sunflower Ave', 'Bachelor', 'Unemployed', 'Completed'),
('G567890123', 'george@example.com', 'pass404', 'George Ho', 1, '1987-07-07', '555-7777', '707 Jasmine Ave', 'Master', 'Employed', 'Completed'),
('H678901234', 'helen@example.com', 'pass505', 'Helen Wu', 0, '1986-08-08', '555-8888', '808 Lavender Ave', 'Bachelor', 'Employed', 'Exempt'),
('I789012345', 'ivy@example.com', 'pass606', 'Ivy Liu', 0, '1994-09-09', '555-9999', '909 Marigold Ave', 'Bachelor', 'Unemployed', 'Completed'),
('J890123456', 'jack@example.com', 'pass707', 'Jack Cheng', 1, '1989-10-10', '555-1010', '1010 Carnation Ave', 'Bachelor', 'Employed', 'Completed');
GO
INSERT INTO Resumes (CandidateID, Title, TitleClassID, Intro, Autobiography, WorkExperience, [Time], [Address], ReleaseYN) VALUES
(1, 'Senior Software Engineer Resume', 'B1', 'Experienced software engineer specializing in backend development', 'Alice has over 8 years of experience...', 'Tech Solutions, Backend Developer...', 'Full-time', '123 Main St', 1),
(2, 'Renewable Energy Consultant Resume', 'B2', 'Expert in renewable energy project consulting', 'Bob has been involved in multiple renewable energy projects...', 'Green Energy, Project Consultant...', 'Full-time', '456 Elm St', 1),
(3, 'Graphic Designer Resume', 'J1', 'Creative graphic designer with a strong portfolio', 'Charlie is skilled in Adobe Creative Suite...', 'Creative Designs, Graphic Designer...', 'Full-time', '789 Oak St', 1),
(4, 'Healthcare Administrator Resume', 'A2', 'Experienced in managing healthcare operations', 'Dana has led teams in hospital administration...', 'Healthcare Plus, Admin Manager...', 'Full-time', '321 Pine St', 1),
(5, 'Financial Analyst Resume', 'H1', 'Analytical financial analyst with a knack for data interpretation', 'Evan has experience in financial modeling...', 'Finance World, Financial Analyst...', 'Full-time', '654 Maple St', 1),
(6, 'Junior IT Support Specialist Resume', 'G1', 'Passionate about IT and helping people solve technical problems', 'Fiona has worked in various IT support roles...', 'Tech Innovators, IT Support...', 'Full-time', '987 Cedar St', 1),
(7, 'Paralegal Resume', 'I1', 'Detail-oriented paralegal with experience in legal research', 'George has assisted in multiple high-profile cases...', 'Legal Eagle, Paralegal...', 'Full-time', '147 Birch St', 1),
(8, 'Sales Executive Resume', 'IA', 'Results-driven sales executive with a proven track record', 'Helen has exceeded sales targets consistently...', 'Sales Empire, Sales Executive...', 'Full-time', '258 Spruce St', 1),
(9, 'Digital Marketing Specialist Resume', 'D1', 'Creative digital marketer with experience in SEO and SEM', 'Ivy has managed several successful digital campaigns...', 'Marketing Magic, Marketing Specialist...', 'Full-time', '369 Fir St', 1),
(10, 'Customer Support Agent Resume', 'F1', 'Friendly and efficient customer support agent', 'Jack has provided excellent customer service...', 'Customer First, Support Agent...', 'Full-time', '852 Ash St', 1);
GO
INSERT INTO ResumeOpeningRecords (ResumeID, OpeningID, CompanyID, CompanyName, OpeningTitle, ApplyDate, LikeYN, InterviewYN, HireYN) VALUES
(1, 1, 1, 'Tech Solutions', 'Senior Software Engineer', '2024-08-01', 1, 1, 0),
(2, 2, 2, 'Green Energy', 'Renewable Energy Consultant', '2024-08-02', 1, 0, 0),
(3, 3, 3, 'Creative Designs', 'Graphic Designer', '2024-08-03', 1, 1, 0),
(4, 4, 4, 'Healthcare Plus', 'Healthcare Administrator', '2024-08-04', 0, 0, 0),
(5, 5, 5, 'Finance World', 'Financial Analyst', '2024-08-05', 1, 1, 1),
(6, 6, 6, 'Tech Innovators', 'Junior IT Support Specialist', '2024-08-06', 1, 0, 0),
(7, 7, 7, 'Legal Eagle', 'Paralegal', '2024-08-07', 0, 0, 0),
(8, 8, 8, 'Sales Empire', 'Sales Executive', '2024-08-08', 1, 1, 0),
(9, 9, 9, 'Marketing Magic', 'Digital Marketing Specialist', '2024-08-09', 1, 1, 0),
(10, 10, 10, 'Customer First', 'Customer Support Agent', '2024-08-10', 1, 0, 0);
GO
INSERT INTO CompanyResumeRecords (CompanyID, ResumeID, LikeYN, InterviewYN, HireYN) VALUES
(1, 1, 1, 1, 0),
(2, 2, 1, 0, 0),
(3, 3, 1, 1, 0),
(4, 4, 0, 0, 0),
(5, 5, 1, 1, 1),
(6, 6, 1, 0, 0),
(7, 7, 0, 0, 0),
(8, 8, 1, 1, 0),
(9, 9, 1, 1, 0),
(10, 10, 1, 0, 0);
GO
INSERT INTO TagClasses (TagClass) VALUES
(N'其他'),
('Programming'),
('Design'),
('Management'),
('Marketing'),
('Sales'),
('Support'),
('Finance'),
('Healthcare'),
('Legal'),
('Human Resources');
GO
INSERT INTO Tags (TagClassID, TagName) VALUES
(1, 'Java'),
(1, 'C++'),
(2, 'Adobe Photoshop'),
(3, 'Leadership'),
(4, 'SEO'),
(5, 'Cold Calling'),
(6, 'Technical Support'),
(7, 'Budgeting'),
(8, 'Patient Care'),
(9, 'Legal Research'),
(10, 'Strategic Planning'),
(0, N'TIM102全端班');
GO
INSERT INTO OpeningTags (OpeningID, TagID) VALUES
(1, 1),
(1, 2),
(2, 7),
(2, 12),
(3, 3),
(4, 8),
(5, 7),
(5, 3),
(6, 6),
(7, 9),
(8, 5),
(9, 4),
(10, 6);
GO
INSERT INTO ResumeTags (ResumeID, TagID) VALUES
(1, 1),
(1, 2),
(2, 7),
(3, 3),
(4, 8),
(4, 12),
(5, 7),
(6, 6),
(7, 9),
(7, 11),
(8, 5),
(9, 4),
(10, 6);
GO
INSERT INTO PricingPlans (Title, Intro, Duration, Price, Discount) VALUES
(N'7天方案', N'7天方案', 7, 380, 1.00),
(N'30天方案', N'30天方案', 30, 4200, 1.00),
(N'60天方案', N'60天方案', 60, 6800, 1.00),
(N'90天方案', N'90天方案', 90, 8400, 1.00),
(N'180天方案', N'180天方案', 180, 15750, 1.00),
(N'365天方案', N'365天方案', 365, 31500, 1.00);
GO
INSERT INTO CompanyOrders (CompanyID, PlanID, CompanyName, GUINumber, Title, Price, OrderDate, [Status]) VALUES
(1, 1, 'Tech Solutions', 10000001, N'7天方案', 380, GETDATE(), 1),
(2, 2, 'Green Energy', 10000002, N'30天方案', 4200, GETDATE(), 1),
(3, 3, 'Creative Designs', 10000003, N'60天方案', 6800, GETDATE(), 1),
(4, 4, 'Healthcare Plus', 10000004, N'90天方案', 8400, GETDATE(), 1),
(5, 5, 'Finance World', 10000005, N'180天方案', 15750, GETDATE(), 1),
(6, 6, 'Tech Innovators', 10000006, N'365天方案', 31500, GETDATE(), 1),
(7, 6, 'Legal Eagle', 10000007, N'365天方案', 31500, GETDATE(), 1),
(8, 2, 'Sales Empire', 10000008, N'30天方案', 4200, GETDATE(), 1),
(9, 4, 'Marketing Magic', 10000009, N'90天方案', 8400, GETDATE(), 1),
(10, 6, 'Customer First', 10000010, N'365天方案', 31500, GETDATE(), 1);
GO
INSERT INTO Notifications (CompanyID, CandidateID, ResumeID, OpeningID, [Status], SubjectLine, Content, SendDate, AppointmentTime) VALUES
(1, 1, 1, 1, 'Sent', 'Interview Invitation', 'We would like to invite you for an interview...', '2024-08-01', '2024-08-03 10:00:00'),
(2, 2, 2, 2, 'Sent', 'Job Offer', 'We are pleased to offer you the position...', '2024-08-02', '2024-08-05 09:00:00'),
(3, 3, 3, 3, 'Sent', 'Thank You for Your Application', 'Thank you for applying to Creative Designs...', '2024-08-03', NULL),
(4, 4, 4, 4, 'Sent', 'Follow-Up', 'We would like to follow up on your application...', '2024-08-04', NULL),
(5, 5, 5, 5, 'Sent', 'Interview Reschedule', 'Your interview has been rescheduled to...', '2024-08-05', '2024-08-07 14:00:00'),
(6, 6, 6, 6, 'Sent', 'Application Received', 'We have received your application...', '2024-08-06', NULL),
(7, 7, 7, 7, 'Sent', 'Second Interview Invitation', 'We would like to invite you for a second interview...', '2024-08-07', '2024-08-09 11:00:00'),
(8, 8, 8, 8, 'Sent', 'Interview Confirmation', 'Please confirm your interview...', '2024-08-08', NULL),
(9, 9, 9, 9, 'Sent', 'Rejection Letter', 'We regret to inform you...', '2024-08-09', NULL),
(10, 10, 10, 10, 'Sent', 'Offer Confirmation', 'Please confirm your acceptance of the offer...', '2024-08-10', NULL);
GO
INSERT INTO Admins (PersonnelCode, Email, [Password], [Name], Authority) VALUES
(1001, 'admin1@jobsite.com', 'adminpass1', 'Admin One', 1),
(1002, 'admin2@jobsite.com', 'adminpass2', 'Admin Two', 2),
(1003, 'admin3@jobsite.com', 'adminpass3', 'Admin Three', 3),
(1004, 'admin4@jobsite.com', 'adminpass4', 'Admin Four', 1),
(1005, 'admin5@jobsite.com', 'adminpass5', 'Admin Five', 2),
(1006, 'admin6@jobsite.com', 'adminpass6', 'Admin Six', 3),
(1007, 'admin7@jobsite.com', 'adminpass7', 'Admin Seven', 1),
(1008, 'admin8@jobsite.com', 'adminpass8', 'Admin Eight', 2),
(1009, 'admin9@jobsite.com', 'adminpass9', 'Admin Nine', 3),
(1010, 'admin10@jobsite.com', 'adminpass10', 'Admin Ten', 1);
GO
INSERT INTO OpinionLetters (CompanyID, CandidateID, AdminID, Class, SubjectLine, Content, Attachment, [Status]) VALUES
(1, 1, 1, 'Complaint', 'Issue with Job Posting', 'I have an issue with the job posting...', NULL, 0),
(2, 2, 2, 'Feedback', 'Great Service', 'I am happy with the services provided...', NULL, 1),
(3, 3, 3, 'Suggestion', 'Improve Search Feature', 'Can you improve the search feature...', NULL, 0),
(4, 4, 4, 'Inquiry', 'Question About Subscription', 'I have a question about my subscription...', NULL, 1),
(5, 5, 5, 'Complaint', 'Issue with Payment', 'There was a problem with my payment...', NULL, 0),
(6, 6, 6, 'Feedback', 'Excellent Platform', 'Your platform is excellent...', NULL, 1),
(7, 7, 7, 'Suggestion', 'Add More Features', 'Could you add more features...', NULL, 0),
(8, 8, 8, 'Inquiry', 'How to Post a Job', 'How can I post a job...', NULL, 1),
(9, 9, 9, 'Complaint', 'Account Issues', 'I am having trouble accessing my account...', NULL, 0),
(10, 10, 10, 'Feedback', 'Great Customer Support', 'Your customer support is great...', NULL, 1);