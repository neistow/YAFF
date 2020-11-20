create or replace function get_user_by_id(user_id uuid) returns users as
$$
select *
from users u
where u.id = user_id
limit 1;
$$
    language sql;

create or replace function get_user_by_email(user_email varchar) returns users as
$$
select *
from users u
where u.email = user_email
limit 1
$$
    language sql;



create or replace function get_user_posts(user_id uuid) returns setof posts as
$$
select *
from posts p
where p.AuthorId = user_id;
$$
    language sql;