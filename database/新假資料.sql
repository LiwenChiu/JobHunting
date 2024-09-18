﻿USE Duck
GO
INSERT INTO CompanyCategories (CompanyCategoryId, CompanyCategoryName) VALUES
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
INSERT INTO CompanyClasses (CompanyClassId, CompanyCategoryId, CompanyClassName) VALUES
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
INSERT INTO Companies (GUINumber, [Password], CompanyName, CompanyClassId, [Address], Intro, Benefits, Picture, ContactName, ContactPhone, ContactEmail, [Status], [Date], Deadline) VALUES
(N'12345678', N'password1', N'農業公司', 'A1', N'台北市大安區', N'專注於有機農產品', N'提供員工健康保險', NULL, N'張三', N'0987654321', N'zhangsan@agri.com', 1, '2024-01-01', '2024-12-31'),
(N'23456789', N'password2', N'漁業科技公司', 'A3', N'高雄市苓雅區', N'致力於水產養殖技術', N'享有彈性工作時間', NULL, N'李四', N'0976543210', N'lisifish@tech.com', 1, '2023-11-01', '2024-10-31'),
(N'34567890', N'password3', N'化學製品公司', 'C4', N'新竹市東區', N'生產高效能化學品', N'提供員工宿舍', NULL, N'王五', N'0965432109', N'wango@chem.com', 1, '2023-09-15', '2024-09-14'),
(N'45678901', N'password4', N'電力供應公司', 'D1', N'台中市南屯區', N'主要提供綠能電力', N'享有年度獎金', NULL, N'趙六', N'0954321098', N'zhaoli@power.com', 1, '2023-07-30', '2024-07-29'),
(N'56789012', N'password5', N'金融控股公司', 'H1', N'台北市信義區', N'全方位金融服務', N'提供進修補助', NULL, N'孫七', N'0943210987', N'sunsun@fin.com', 1, '2023-06-20', '2024-06-19'),
(N'12345678', N'password123', N'台灣電子科技股份有限公司', 'C1', N'台北市信義區信義路五段7號', N'專注於科技創新，領先業界的解決方案提供者', N'提供完整的福利、員工旅遊、績效獎金',NULL, N'王大明', N'0912345678', N'daming@example.com', 1, '2024-09-13', '2024-12-31'),
(N'87654321', N'abc456789', N'永信藥品工業股份有限公司', 'C8', N'台中市西屯區工業路123號', N'全球領先的藥品研發與製造企業', N'健全的升遷制度、完整的健康保險',NULL, N'李雅婷', N'0922333444', N'yating@example.com', 0, '2024-08-05', '2024-11-01'),
(N'11223344', N'qwerty12345', N'盛世建設有限公司', 'E1', N'新北市板橋區文化路一段100號', N'建築與工程的領導者', N'員工宿舍、健身房與醫療保險',NULL, N'林建華', N'0988776655', N'jianhua@example.com', 1, '2024-07-25', '2024-10-31'),
(N'33445566', N'789xyz321', N'華南金融控股股份有限公司', 'H1', N'高雄市苓雅區三多一路500號', N'提供全方位的金融服務', N'高額年終獎金、股票分紅',NULL, N'鄭文斌', N'0933445566', N'wenbin@example.com', 1, '2024-06-30', '2024-12-01'),
(N'55667788', N'pass987654', N'大亞物流股份有限公司', 'G1', N'台中市北區中清路二段200號', N'全國最大物流配送公司', N'年終雙薪、優質的醫療保險',NULL, N'蔡志明', N'0955443322', N'zhiming@example.com', 0, '2024-05-10', '2024-09-30'),
(N'66778899', N'pwd123987', N'欣欣百貨股份有限公司', 'F3', N'台北市中正區重慶南路二段50號', N'提供優質的購物體驗', N'多元津貼、分紅與股票配股',NULL, N'劉婉婷', N'0911223344', N'wanting@example.com', 1, '2024-04-20', '2024-08-15'),
(N'22334455', N'xyzpass4321', N'群益證券股份有限公司', 'H3', N'台北市大安區敦化南路一段100號', N'專業的證券交易與投資服務', N'高額年終獎金與全額補助保險',NULL, N'黃志忠', N'0977665544', N'zhizhong@example.com', 1, '2024-03-05', '2024-07-30'),
(N'44556677', N'abc987123', N'台灣食品製造股份有限公司', 'C1', N'新北市新莊區中山路三段300號', N'領先的食品製造與分銷商', N'提供免費午餐與交通補助',NULL, N'陳美麗', N'0966554433', N'meili@example.com', 0, '2024-02-28', '2024-06-15'),
(N'99887766', N'asd123456', N'巨人娛樂股份有限公司', 'J7', N'台南市東區大同路一段1號', N'全台最大的娛樂服務平台', N'員工優惠、股票選擇權',NULL, N'何志強', N'0955447766', N'zhichang@example.com', 1, '2024-01-15', '2024-05-20'),
(N'55663322', N'pwd123654', N'遠東石油股份有限公司', 'B1', N'高雄市前鎮區成功路120號', N'專注於能源開發與供應', N'高額薪資、免費體檢',NULL, N'周大海', N'0933557788', N'dahai@example.com', 0, '2023-12-10', '2024-03-31');
GO
INSERT INTO TitleCategories (TitleCategoryName) VALUES
(N'其他'),
(N'管理類'),
(N'技術類'),
(N'行政類'),
(N'市場行銷類'),
(N'客服類'),
(N'設計類'),
(N'人力資源類'),
(N'財務類'),
(N'資訊類'),
(N'研發類'),
(N'法律類'),
(N'教育類'),
(N'醫療類'),
(N'物流類'),
(N'建築類'),
(N'工程類'),
(N'娛樂類'),
(N'環保類'),
(N'藝術類'),
(N'社會服務類');
GO
INSERT INTO TitleClasses (TitleCategoryId, TitleClassName) VALUES
(1, N'總經理'),
(1, N'副總經理'),
(2, N'工程師'),
(2, N'軟體開發師'),
(3, N'行政助理'),
(3, N'秘書'),
(4, N'市場行銷專員'),
(4, N'品牌經理'),
(5, N'客服代表'),
(5, N'客服主管'),
(6, N'平面設計師'),
(6, N'多媒體設計師'),
(7, N'人資專員'),
(8, N'會計'),
(8, N'財務分析師'),
(9, N'系統分析師'),
(10, N'研發專員'),
(11, N'律師'),
(12, N'講師');
GO
INSERT INTO Openings (CompanyId, Title, [Address], [Description], Degree, Benefits, InterviewYN, SalaryMax, SalaryMin, [Time], ContactName, ContactPhone, ContactEmail, ReleaseYN) VALUES
(1, N'農業技術專員', N'台北市大安區', N'專注於農業技術研發', N'大學', N'員工享有交通補貼', 1, 70000, 50000, N'全職', N'張三', N'0987654321', N'zhangsan@agri.com', 1),
(2, N'水產養殖技術專家', N'高雄市苓雅區', N'負責水產養殖技術研發', N'碩士', N'公司提供進修機會', 1, 85000, 60000, N'全職', N'李四', N'0976543210', N'lisifish@tech.com', 1),
(3, N'化學研發工程師', N'新竹市東區', N'參與化學製品研發工作', N'碩士', N'提供專業進修補助', 1, 90000, 65000, N'全職', N'王五', N'0965432109', N'wango@chem.com', 1),
(4, N'電力供應工程師', N'台中市南屯區', N'負責電力設備管理', N'大學', N'享有年度獎金', 1, 75000, 55000, N'全職', N'趙六', N'0954321098', N'zhaoli@power.com', 1),
(5, N'金融分析師', N'台北市信義區', N'提供投資分析和報告', N'碩士', N'提供進修補助', 1, 95000, 70000, N'全職', N'孫七', N'0943210987', N'sunsun@fin.com', 1),
(6, N'軟體工程師', N'台北市信義區信義路五段7號', N'負責軟體開發與維護，與團隊合作完成專案', N'資訊相關學系', N'年度分紅、彈性上下班時間', 1, 80000, 60000, N'09:00-18:00', N'王大明', N'0912345678', N'daming@example.com', 1),
(7, N'製藥品管工程師', N'台中市西屯區工業路123號', N'負責製藥過程品質管控，確保產品合規', N'化工相關學系', N'健全的保險福利、年終獎金', 1, 70000, 50000, N'08:30-17:30', N'李雅婷', N'0922333444', N'yating@example.com', 1),
(8, N'建築設計師', N'新北市板橋區文化路一段100號', N'負責建築設計與規劃，參與大型建設專案', N'建築學系', N'專案獎金、健康檢查', 1, 90000, 65000, N'09:00-18:00', N'林建華', N'0988776655', N'jianhua@example.com', 1),
(9, N'金融分析師', N'高雄市苓雅區三多一路500號', N'負責金融市場分析與投資策略研究', N'經濟或金融相關學系', N'年終分紅、完整的保險福利', 1, 85000, 70000, N'09:00-17:30', N'鄭文斌', N'0933445566', N'wenbin@example.com', 1),
(10, N'物流專員', N'台中市北區中清路二段200號', N'負責貨物調度與物流安排，確保運送順暢', N'物流管理相關學系', N'績效獎金、全額保險', 0, 60000, 45000, N'08:00-17:00', N'蔡志明', N'0955443322', N'zhiming@example.com', 1),
(11, N'行銷專員', N'台北市中正區重慶南路二段50號', N'負責市場調查與推廣策略執行', N'行銷或商管相關學系', N'多元獎金制度、員工旅遊', 1, 75000, 50000, N'09:00-18:00', N'劉婉婷', N'0911223344', N'wanting@example.com', 1),
(12, N'證券交易員', N'台北市大安區敦化南路一段100號', N'負責證券交易操作與市場監控', N'金融或經濟相關學系', N'高額年終獎金、專業進修機會', 1, 80000, 60000, N'08:30-17:30', N'黃志忠', N'0977665544', N'zhizhong@example.com', 1),
(13, N'食品安全檢驗員', N'新北市新莊區中山路三段300號', N'負責食品安全檢驗與質量控管', N'食品科學或相關學系', N'健康保險、節慶獎金', 0, 68000, 50000, N'09:00-18:00', N'陳美麗', N'0966554433', N'meili@example.com', 1),
(14, N'遊戲開發工程師', N'台南市東區大同路一段1號', N'負責遊戲系統開發與維護', N'資訊或遊戲設計相關學系', N'遊戲免費試玩、年度獎金', 1, 85000, 60000, N'09:30-18:30', N'何志強', N'0955447766', N'zhichang@example.com', 1),
(15, N'石油勘探工程師', N'高雄市前鎮區成功路120號', N'負責石油勘探與技術分析，參與海外項目', N'地質或能源相關學系', N'高額薪資、免費住宿', 0, 95000, 70000, N'08:00-17:00', N'周大海', N'0933557788', N'dahai@example.com', 1);
GO
INSERT INTO OpeningTitleClasses (OpeningId, TitleClassId)
VALUES
(1, 1),
(1, 2),
(2, 3),
(2, 4),
(3, 5),
(3, 6),
(4, 7),
(4, 8),
(5, 9),
(5, 10),
(6, 1),
(7, 2),
(8, 3),
(9, 4),
(10, 5);
GO
INSERT INTO Candidates (NationalId, Email, [Password], [Name], Sex, Birthday, Headshot, TitleClass, Phone, [Address], Degree, EmploymentStatus, MilitaryService)
VALUES
(N'A123456789', N'john@example.com', N'password123', N'王小明', 1, '1985-07-15', NULL, N'軟體工程師', N'0988111222', N'台北市大安區和平東路', N'大學', N'在職中', N'役畢'),
(N'A987654321', N'jane@example.com', N'password456', N'陳美麗', 0, '1990-03-25', NULL, N'行銷專員', N'0922333444', N'新北市板橋區', N'碩士', N'待業中', N'無役'),
(N'A567891234', N'alex@example.com', N'password789', N'林建華', 1, '1980-01-05', NULL, N'建築設計師', N'0911223344', N'台中市西屯區', N'大學', N'在職中', N'役畢'),
(N'A223344556', N'mary@example.com', N'password111', N'李雅婷', 0, '1988-05-17', NULL, N'金融分析師', N'0933445566', N'台南市東區', N'碩士', N'在職中', N'無役'),
(N'A998877665', N'bob@example.com', N'password222', N'張志強', 1, '1982-09-10', NULL, N'物流專員', N'0955443322', N'高雄市三民區', N'大學', N'待業中', N'役畢'),
(N'A112233445', N'james@example.com', N'password333', N'周大海', 1, '1975-12-20', NULL, N'石油勘探工程師', N'0933557788', N'基隆市', N'碩士', N'在職中', N'未役'),
(N'A554433221', N'lucy@example.com', N'password444', N'劉婉婷', 0, '1992-08-30', NULL, N'證券交易員', N'0977665544', N'桃園市', N'碩士', N'待業中', N'無役'),
(N'A667788990', N'angela@example.com', N'password555', N'黃志忠', 1, '1984-11-12', NULL, N'食品安全檢驗員', N'0966554433', N'苗栗縣', N'大學', N'在職中', N'役畢'),
(N'A334455667', N'tom@example.com', N'password666', N'蔡志明', 1, '1987-02-14', NULL, N'遊戲開發工程師', N'0955447766', N'新竹市', N'大學', N'在職中', N'免役'),
(N'A556677889', N'sam@example.com', N'password777', N'何志強', 1, '1991-06-25', NULL, N'證券交易員', N'0933221122', N'宜蘭縣', N'大學', N'在職中', N'役畢');
GO
INSERT INTO Resumes (CandidateId, Title, Intro, Headshot, Autobiography, WorkExperience, Certification, [Time], [Address], ReleaseYN)
VALUES
(1, N'軟體工程師履歷', N'具備5年開發經驗', NULL, N'熱愛編程，曾參與大型專案', N'台北軟體公司工作3年', NULL, N'全職', N'台北市大安區', 1),
(2, N'行銷專員履歷', N'具備市場推廣能力', NULL, N'精通市場調查與數據分析', N'曾在知名行銷公司工作2年', NULL, N'全職', N'新北市板橋區', 1),
(3, N'建築設計師履歷', N'設計創新，符合需求', NULL, N'曾參與多個國內外建築專案', N'在知名建築公司工作4年', NULL, N'全職', N'台中市西屯區', 1),
(4, N'金融分析師履歷', N'深入分析金融市場', NULL, N'精通投資策略與風險管理', N'在大型金融機構工作3年', NULL, N'全職', N'台南市東區', 1),
(5, N'物流專員履歷', N'物流規劃經驗豐富', NULL, N'具備貨物調度與管理能力', N'曾在大型物流公司工作5年', NULL, N'全職', N'高雄市三民區', 1),
(6, N'石油勘探工程師履歷', N'具備能源勘探技術', NULL, N'曾參與多個海外石油項目', N'有超過8年的工作經驗', NULL, N'全職', N'基隆市', 1),
(7, N'證券交易員履歷', N'專注於金融交易', NULL, N'負責市場監控與操作', N'曾在大型證券公司工作3年', NULL, N'全職', N'桃園市', 1),
(8, N'食品安全檢驗員履歷', N'具備食品檢驗專業', NULL, N'精通質量控制與檢驗流程', N'曾在知名食品公司工作2年', NULL, N'全職', N'苗栗縣', 1),
(9, N'遊戲開發工程師履歷', N'遊戲開發經驗豐富', NULL, N'曾參與多款知名遊戲的開發', N'在遊戲公司工作3年', NULL, N'全職', N'新竹市', 1),
(10, N'證券交易員履歷', N'負責市場操作與交易', NULL, N'在證券行業有豐富經驗', N'曾在證券公司工作2年', NULL, N'全職', N'宜蘭縣', 1);
GO
INSERT INTO ResumeTitleClasses (ResumeId, TitleClassId)
VALUES
(1, 1),
(2, 2),
(3, 3),
(4, 4),
(5, 5),
(6, 6),
(7, 7),
(8, 8),
(9, 9),
(10, 10);
GO
INSERT INTO ResumeOpeningRecords (ResumeId, OpeningId, CompanyId, CompanyName, OpeningTitle, ApplyDate, InterviewYN, HireYN)
VALUES
(1, 1, 1, N'農業公司', N'農業技術專員', '2024-08-15', 1, 0),
(2, 2, 2, N'漁業科技公司', N'水產養殖技術專家', '2024-08-16', 0, 0),
(3, 3, 3, N'化學製品公司', N'化學研發工程師', '2024-08-17', 1, 1),
(4, 4, 4, N'電力供應公司', N'電力供應工程師', '2024-08-18', 1, 0),
(5, 5, 5, N'金融控股公司', N'金融分析師', '2024-08-19', 0, 0),
(6, 6, 6, N'台灣電子科技股份有限公司', N'軟體工程師', '2024-08-20', 1, 1),
(7, 7, 7, N'永信藥品工業股份有限公司', N'製藥品管工程師', '2024-08-21', 0, 0),
(8, 8, 8, N'盛世建設有限公司', N'建築設計師', '2024-08-22', 1, 0),
(9, 9, 9, N'華南金融控股股份有限公司', N'金融分析師', '2024-08-23', 1, 1),
(10, 10, 10, N'大亞物流股份有限公司', N'物流專員', '2024-08-24', 0, 0);
GO
INSERT INTO CompanyResumeLikeRecords (CompanyId, ResumeId)
VALUES
(1, 1),
(2, 2),
(3, 3),
(4, 4),
(5, 5),
(6, 6),
(7, 7),
(8, 8),
(9, 9),
(10, 10);
GO
INSERT INTO CandidateOpeningLikeRecords (CandidateId, OpeningId)
VALUES
(1, 1),
(2, 2),
(3, 3),
(4, 4),
(5, 5),
(6, 6),
(7, 7),
(8, 8),
(9, 9),
(10, 10),
(1,2),
(1,3),
(1,4),
(1,5),
(1,6),
(1,7),
(1,8),
(1,9),
(1,10);
GO
INSERT INTO TagClasses (TagClassName)
VALUES
(N'其他'),
(N'技術'),
(N'管理'),
(N'行銷'),
(N'設計'),
(N'金融'),
(N'物流'),
(N'能源'),
(N'食品'),
(N'遊戲'),
(N'證券');
GO
INSERT INTO Tags (TagClassId, TagName)
VALUES
(1, N'Python'),
(2, N'專案管理'),
(3, N'SEO'),
(4, N'UI設計'),
(5, N'投資分析'),
(6, N'供應鏈管理'),
(7, N'石油勘探'),
(8, N'食品檢驗'),
(9, N'遊戲開發'),
(10, N'市場監控');
GO
INSERT INTO OpeningTags (OpeningId, TagId)
VALUES
(1, 1),
(2, 2),
(3, 3),
(4, 4),
(5, 5),
(6, 6),
(7, 7),
(8, 8),
(9, 9),
(10, 10);
GO
INSERT INTO ResumeTags (ResumeId, TagId)
VALUES
(1, 1),
(2, 2),
(3, 3),
(4, 4),
(5, 5),
(6, 6),
(7, 7),
(8, 8),
(9, 9),
(10, 10);
GO
INSERT INTO PricingPlans (Title, Intro, Duration, Price, Discount, [Status]) VALUES
(N'基本計畫', N'包含基本功能', 30, 1000, 0.9, 1),
(N'進階計畫', N'包括所有基本計畫功能及進階功能', 60, 1800, 0.85, 1),
(N'專業計畫', N'包括所有功能和專業支援', 90, 2500, 0.8, 1),
(N'企業計畫', N'為企業提供定制解決方案', 365, 10000, 0.75, 1),
(N'入門計畫', N'適合小型業務使用', 30, 800, 0.95, 1),
(N'標準計畫', N'包括大多數功能', 60, 1500, 0.88, 1),
(N'高級計畫', N'提供高級功能及支持', 90, 2200, 0.82, 1),
(N'年度計畫', N'每年更新一次', 365, 9500, 0.77, 1),
(N'試用計畫', N'免費試用30天', 30, 0, 1, 0),
(N'專業企業計畫', N'針對大型企業的專業方案', 365, 15000, 0.7, 1),
(N'進階標準計畫', N'進階功能的標準版本', 60, 1700, 0.84, 1),
(N'高端計畫', N'所有功能和服務的高端版本', 90, 2700, 0.78, 1),
(N'服務業計畫', N'專為服務業設計', 30, 1200, 0.92, 1),
(N'教育計畫', N'針對教育機構的特別方案', 60, 1600, 0.87, 1),
(N'健康計畫', N'專為健康領域設計', 90, 2000, 0.83, 1),
(N'商業計畫', N'針對中小企業的解決方案', 365, 10500, 0.72, 1),
(N'技術支持計畫', N'包括技術支持服務', 30, 900, 0.9, 1),
(N'綜合計畫', N'綜合所有功能', 60, 1400, 0.86, 1),
(N'完整計畫', N'提供完整功能和服務', 90, 2400, 0.8, 1),
(N'定制計畫', N'根據需求定制的計畫', 365, 12000, 0.73, 1);

GO
INSERT INTO CompanyOrders (CompanyId, PlanId, CompanyName, GUINumber, Title, Price, OrderDate, PayDate, Duration, [Status]) VALUES
(1, 1, N'農業公司', N'12345678', N'基本計畫訂單', 1000, '2024-08-01', '2024-08-02', 30, 1),
(2, 2, N'漁業科技公司', N'23456789', N'進階計畫訂單', 1800, '2024-08-03', '2024-08-04', 60, 1),
(3, 3, N'化學製品公司', N'34567890', N'專業計畫訂單', 2500, '2024-08-05', '2024-08-06', 90, 1),
(4, 4, N'電力供應公司', N'45678901', N'企業計畫訂單', 10000, '2024-08-07', '2024-08-08', 365, 1),
(5, 5, N'金融控股公司', N'56789012', N'入門計畫訂單', 800, '2024-08-09', '2024-08-10', 30, 1),
(6, 6, N'台灣電子科技股份有限公司', N'12345678', N'標準計畫訂單', 1500, '2024-08-11', '2024-08-12', 60, 1),
(7, 7, N'永信藥品工業股份有限公司', N'87654321', N'高級計畫訂單', 2200, '2024-08-13', '2024-08-14', 90, 1),
(8, 8, N'盛世建設有限公司', N'11223344', N'年度計畫訂單', 9500, '2024-08-15', '2024-08-16', 365, 1),
(9, 9, N'華南金融控股股份有限公司', N'33445566', N'試用計畫訂單', 0, '2024-08-17', '2024-08-18', 30, 0),
(10, 10, N'大亞物流股份有限公司', N'55667788', N'專業企業計畫訂單', 15000, '2024-08-19', '2024-08-20', 365, 1),
(11, 11, N'欣欣百貨股份有限公司', N'66778899', N'進階標準計畫訂單', 1700, '2024-08-21', '2024-08-22', 60, 1),
(12, 12, N'群益證券股份有限公司', N'22334455', N'高端計畫訂單', 2700, '2024-08-23', '2024-08-24', 90, 1),
(13, 13, N'台灣食品製造股份有限公司', N'44556677', N'服務業計畫訂單', 1200, '2024-08-25', '2024-08-26', 30, 1),
(14, 14, N'巨人娛樂股份有限公司', N'99887766', N'教育計畫訂單', 1600, '2024-08-27', '2024-08-28', 60, 1),
(15, 15, N'遠東石油股份有限公司', N'55663322', N'健康計畫訂單', 2000, '2024-08-29', '2024-08-30', 90, 1),
(9, 16, N'華南金融控股股份有限公司', N'33445566', N'商業計畫訂單', 10500, '2024-08-31', '2024-09-01', 365, 1),
(4, 17, N'電力供應公司', N'45678901', N'技術支持計畫訂單', 900, '2024-09-02', '2024-09-03', 30, 1),
(5, 18, N'金融控股公司', N'56789012', N'綜合計畫訂單', 1400, '2024-09-04', '2024-09-05', 60, 1),
(7, 19, N'永信藥品工業股份有限公司', N'87654321', N'完整計畫訂單', 2400, '2024-09-06', '2024-09-07', 90, 1),
(2, 20, N'漁業科技公司', N'23456789', N'定制計畫訂單', 12000, '2024-09-08', '2024-09-09', 365, 1);
GO
INSERT INTO Notifications (CompanyId, CandidateId, OpeningId, ResumeId, [Status], SubjectLine, Content, SendDate, AppointmentDate, AppointmentTime,ReplyYN,Reply,ReplyFirstYN) VALUES
(1, 1, 1, 1, N'待處理', N'您的申請已收到，請等待後續通知。', N'hhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhh', '2024-08-01', NULL, NULL, 0, NULL,0),
(2, 2, 2, 2, N'面試邀請', N'我們邀請您參加面試，請查看詳情。', N'ssssssssssssssssssssssssssssssssssssss', '2024-08-02', '2024-08-05','09:00:00',1,'感恩，我會前往',1),
(3, 3, 3, 3, N'申請確認', N'我們已收到您的申請，我們將在3個工作日內聯繫您。', N'agegefFSFDSDFDSEF', '2024-08-03', NULL, NULL,1,'感恩，我會前往',1),
(4, 4, 4, 4, N'面試確認', N'您的面試已確認，請按照下列時間參加。', N'Edfvcdssdcgedsfgstrefdsgtrsef', '2024-08-04', '2024-08-06', '10:00:00',0, NULL,0),
(5, 5, 5, 5, N'審核進度', N'您的申請正在審核中，請耐心等待。', N'EAFDfgdsghdjgfrdesdhgfgfds', '2024-08-05', NULL, NULL,1,'感恩，我會前往',1),
(6, 6, 6, 6, N'面試安排', N'面試安排已完成，請準時到達。', N'sdfaewfdsFESDeSDedesFF', '2024-08-06', '2024-08-07', '14:00:00',0, NULL,0),
(7, 7, 7, 7, N'面試結果', N'您的面試結果已出，請查看詳細資訊。', N'DDSTRHRSHREGDSSDf', '2024-08-07', NULL, NULL,0, NULL,0),
(8, 8, 8, 8, N'申請回覆', N'我們已回覆您的申請，請查看郵件。', N'aremyse4567tkf.65erghj65', '2024-08-08', NULL, NULL,0, NULL,0),
(9, 9, 9, 9, N'面試邀請', N'您已被選中參加面試，請確認參加。', N'WRTU645EHTJ,I75U6EHT756urthdretyj6y5esrhghdyu6y5erty6453wrhgdmcj,yfu6y5erg', '2024-08-09', '2024-08-10', '15:00:00',1,'抱歉，我不會前往',1),
(10, 10, 10, 10, N'申請狀態更新', N'您的申請狀態已更新，請查看。', N'jgdhmjtdkyrj kurytergd m7urgf mudtr hg tydyergfdxhdjyhfdbvngftr', '2024-08-10', NULL, NULL,1,'感恩，我會前往',1),
(11, 1, 11, 1, N'面試通知', N'面試時間已安排，請準時參加。', N'asgehrehtjhz', '2024-08-11', '2024-08-12', '11:00:00',0, NULL,0),
(12, 2, 12, 2, N'結果通知', N'我們已經完成了面試，請查看結果。', N'eehte5454rdfgr3', '2024-08-12', NULL, NULL,0, NULL,0),
(13, 3, 13, 3, N'申請確認', N'我們已確認您的申請，請耐心等待。', N'24354ytujmhngesr', '2024-08-13', NULL, NULL,0,NULL,0),
(14, 4, 14, 4, N'面試安排', N'面試安排已完成，請按時參加。', N'2345ytyresdgnbmhgfdresdx', '2024-08-14', '2024-08-15', '16:00:00',1,'感恩，我會前往',1),
(15, 5, 15, 5, N'進度更新', N'您的申請進度已更新，請查看詳情。', N'salkjbgikjsfdks;ikjsf', '2024-08-15', NULL, NULL,0, NULL,0),
(2, 6, 2, 6, N'面試邀請', N'我們邀請您參加面試，請查看詳細安排。', N'dhgnyhtddjytywrsfng', '2024-08-16', '2024-08-17', '09:30:00',1,'感恩，我會前往',1),
(13, 7, 13, 7, N'面試通知', N'面試時間已確認，請準時參加。', N'TGyajstjyhdf', '2024-08-17', '2024-08-18', '13:00:00',0, NULL,0),
(8, 8, 8, 8, N'申請結果', N'我們已經審核您的申請，請查看結果。', N'Sfgdzrggnhfstrhsfb', '2024-08-18', NULL, NULL,1,'感恩，我會前往',1),
(5, 9, 5, 9, N'面試安排', N'面試時間已安排，請務必準時。', N'awdfgcgrfftrgs', '2024-08-19', '2024-08-20', '10:30:00',1,'感恩，我會前往',1),
(10, 10, 10, 10, N'申請狀態', N'您的申請狀態已更新，請查看。', N'sgfgegsfh', '2024-08-20', NULL, NULL,0,NULL,0);
GO
INSERT INTO Admins (PersonnelCode, Email, [Password], [Name], Authority, [Status]) VALUES
(1001, N'admin1@example.com', N'password1', N'張三', 1, 1),
(1002, N'admin2@example.com', N'password2', N'李四', 2, 1),
(1003, N'admin3@example.com', N'password3', N'王五', 3, 1),
(1004, N'admin4@example.com', N'password4', N'趙六', 1, 1),
(1005, N'admin5@example.com', N'password5', N'錢七', 2, 1),
(1006, N'admin6@example.com', N'password6', N'孫八', 3, 1),
(1007, N'admin7@example.com', N'password7', N'周九', 1, 1),
(1008, N'admin8@example.com', N'password8', N'吴十', 2, 1),
(1009, N'admin9@example.com', N'password9', N'鄭十一', 3, 1),
(1010, N'admin10@example.com', N'password10', N'王十二', 1, 1),
(1011, N'admin11@example.com', N'password11', N'李十三', 2, 1),
(1012, N'admin12@example.com', N'password12', N'張十四', 3, 1),
(1013, N'admin13@example.com', N'password13', N'趙十五', 1, 1),
(1014, N'admin14@example.com', N'password14', N'錢十六', 2, 1),
(1015, N'admin15@example.com', N'password15', N'孫十七', 3, 1),
(1016, N'admin16@example.com', N'password16', N'周十八', 1, 1),
(1017, N'admin17@example.com', N'password17', N'吴十九', 2, 1),
(1018, N'admin18@example.com', N'password18', N'鄭二十', 3, 1),
(1019, N'admin19@example.com', N'password19', N'王二十一', 1, 1),
(1020, N'admin20@example.com', N'password20', N'李二十二', 2, 1);
GO
INSERT INTO OpinionLetters (CompanyId, CandidateId, AdminId, Class, SubjectLine, Content, Attachment, [Status], SendTime) VALUES
(1, NULL, 1, N'系統反饋', N'系統故障報告', N'系統在提交簡歷時出現錯誤，請檢查並修復。', NULL, 1, '2024-08-01 10:00:00'),
(2, NULL, NULL, N'功能建議', N'功能改進建議', N'建議增加一個篩選功能以提高搜索效率。', NULL, 0, '2024-08-03 10:00:00'),
(3, NULL, 3, N'帳戶問題', N'帳戶登錄問題', N'無法登錄帳戶，請檢查帳戶設置。', NULL, 1, '2024-09-01 10:00:00'),
(NULL, 2, NULL, N'界面建議', N'界面設計建議', N'希望改進用戶界面，使其更具可用性。', NULL, 0, '2024-08-15 10:00:00'),
(NULL, 3, 5, N'客服反饋', N'客服服務評價', N'客服服務態度良好，但回應速度可以更快。', NULL, 1, '2024-09-01 10:00:00'),
(6, NULL, 6, N'系統錯誤', N'系統錯誤報告', N'提交簡歷時出現系統錯誤，請儘快修復。', NULL, 1, '2024-07-02 10:00:00'),
(7, NULL, NULL, N'功能請求', N'新功能請求', N'希望在系統中增加一個追蹤應聘進度的功能。', NULL, 0, '2024-05-25 10:00:00'),
(NULL, 5, 8, N'反饋意見', N'網站功能建議', N'網站功能正常，但希望能增加更多的幫助文檔。', NULL, 1, '2024-04-01 10:00:00'),
(9, NULL, 9, N'報告問題', N'報告生成問題', N'報告生成過程中遇到錯誤，請檢查系統。', NULL, 1, '2024-06-01 10:00:00'),
(10, NULL, NULL, N'服務建議', N'服務改進建議', N'建議改進服務響應速度，以提高用戶滿意度。', NULL, 0, '2024-08-04 10:00:00'),
(11, NULL, 11, N'意見反饋', N'系統性能反饋', N'系統性能良好，但在高峰期時偶爾會出現延遲。', NULL, 1, '2024-06-29 10:00:00'),
(12, NULL, 12, N'安全問題', N'安全漏洞報告', N'發現系統存在安全漏洞，請儘快處理。', NULL, 1, '2024-02-01 10:00:00'),
(NULL, 9, NULL, N'功能請求', N'增加新功能請求', N'希望增加一個自定義報表的功能。', NULL, 0, '2024-06-02 10:00:00'),
(14, NULL, 14, N'網站建議', N'網站改進建議', N'網站界面需要進一步優化以提高用戶體驗。', NULL, 1, '2024-09-13 10:00:00'),
(15, NULL, 15, N'登錄問題', N'登錄問題反饋', N'在某些瀏覽器中無法登錄，請修正。', NULL, 1, '2024-08-015 10:00:00'),
(NULL, 10, NULL, N'系統建議', N'系統功能建議', N'建議在系統中增加多語言支持。', NULL, 0, '2024-08-06 10:00:00'),
(NULL, 1, 17, N'客服問題', N'客服反饋', N'客服回應及時，但解決問題的能力有待提高。', NULL, 1, '2024-08-13 08:00:00'),
(8, NULL, NULL, N'功能建議', N'功能改進建議', N'希望能增加用戶活動記錄功能。', NULL, 0, '2024-08-13 10:00:00'),
(5, NULL, 19, N'報告問題', N'報告生成問題反饋', N'生成報告時出現錯誤，請查明原因並修復。', NULL, 1, '2024-08-09 10:00:00'),
(NULL, 5, NULL, N'系統建議', N'系統優化建議', N'建議進一步優化系統性能，以提高處理速度。', NULL, 0, '2024-07-025 10:00:00');

GO
INSERT INTO AdminRecords (AdminId, Task, CRUD, [Time]) VALUES
(1, N'創建公司帳戶', N'CREATE', '2024-08-01 10:00:00'),
(2, N'更新公司資料', N'UPDATE', '2024-08-02 11:00:00'),
(3, N'刪除公司帳戶', N'DELETE', '2024-08-03 12:00:00'),
(4, N'檢查申請狀態', N'READ', '2024-08-04 13:00:00'),
(5, N'發送面試邀請', N'CREATE', '2024-08-05 14:00:00'),
(6, N'修改面試安排', N'UPDATE', '2024-08-06 15:00:00'),
(7, N'審核聘用通知', N'READ', '2024-08-07 16:00:00'),
(8, N'發送申請確認', N'CREATE', '2024-08-08 17:00:00'),
(9, N'更新聘用狀態', N'UPDATE', '2024-08-09 18:00:00'),
(10, N'查看面試結果', N'READ', '2024-08-10 19:00:00'),
(11, N'創建新管理員', N'CREATE', '2024-08-11 20:00:00'),
(12, N'更新管理員資料', N'UPDATE', '2024-08-12 21:00:00'),
(13, N'刪除管理員帳戶', N'DELETE', '2024-08-13 22:00:00'),
(14, N'檢查應聘者資料', N'READ', '2024-08-14 23:00:00'),
(15, N'發送通知信件', N'CREATE', '2024-08-15 10:00:00'),
(16, N'更新通知狀態', N'UPDATE', '2024-08-16 11:00:00'),
(17, N'查看應聘者評價', N'READ', '2024-08-17 12:00:00'),
(18, N'發送聘用信', N'CREATE', '2024-08-18 13:00:00'),
(19, N'修改聘用安排', N'UPDATE', '2024-08-19 14:00:00'),
(20, N'刪除聘用通知', N'DELETE', '2024-08-20 15:00:00');
