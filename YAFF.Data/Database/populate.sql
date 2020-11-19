insert into Users (Id, Nickname, RegistrationDate, Email, EmailConfirmed, IsBanned, PasswordHash)
values ('4CE902D7-485A-4D5B-AB59-6A3F0A7A8F89', 'Tester', now(), 'tester@gmail.com', true, false, ' ');

insert into Posts (Id, Title, Body, DatePosted, AuthorId)
values ('EF9D9F9C-1072-4518-BD5F-8C00BAEB2657', 'Post 1', 'Test 1', now(), '4CE902D7-485A-4D5B-AB59-6A3F0A7A8F89'),
       ('DC215D5B-141B-40E9-A385-7D8FEF432D07', 'Post 2', 'Test 2', now(), '4CE902D7-485A-4D5B-AB59-6A3F0A7A8F89'),
       ('ECA42E62-23E7-4B56-9A7A-0F29932A9ABF', 'Post 3', 'Test 3', now(), '4CE902D7-485A-4D5B-AB59-6A3F0A7A8F89');