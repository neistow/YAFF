drop schema if exists public cascade;
create schema public;

drop table if exists Photos;
create table Photos
(
    Id          uuid
        primary key,
    Filename    text not null,
    ThumbnailId uuid
        constraint photos__thumb__fk
            references Photos on delete set null
);


drop table if exists Users;
create table Users
(
    Id               uuid
        primary key,
    Nickname         varchar(255) not null,
    RegistrationDate timestamp    not null,
    Email            varchar(255) not null,
    EmailConfirmed   bool         not null default false,
    IsBanned         bool         not null default false,
    BanLiftDate      timestamp,
    PasswordHash     text         not null,
    AvatarId         uuid
        constraint users_avatar__fk
            references Photos on delete cascade
);


drop table if exists RefreshTokens;
create table RefreshTokens
(
    Id          uuid primary key,
    Token       text      not null,
    DateCreated timestamp not null,
    DateExpires timestamp not null,
    UserId      uuid
        constraint user_token__fk
            references Users on delete cascade
);


drop table if exists Roles;
create table Roles
(
    Id   uuid primary key,
    Name varchar(255) not null
);


drop table if exists UserRoles;
create table UserRoles
(
    RoleId uuid
        constraint user_role__role__fk
            references Roles on delete restrict,
    UserId uuid
        constraint user_role__user__fk
            references Users on delete cascade,
    constraint user_role__pk
        primary key (RoleId, UserId)
);

drop table if exists Posts;
create table Posts
(
    Id         uuid
        primary key,
    Title      varchar(255) not null,
    Body       text         not null,
    DatePosted timestamp    not null,
    DateEdited timestamp,
    AuthorId   uuid
        constraint post__author__fk
            references Users on delete cascade
);


drop table if exists PostComments;
create table PostComments
(
    Id            uuid
        primary key,
    PostId        uuid
        constraint comment__post__fk
            references Posts on delete cascade,
    AuthorId      uuid
        constraint comment__author__fk
            references Users on delete cascade,
    Body          text      not null,
    DateCommented timestamp not null,
    DateEdited    timestamp,
    ReplyTo       uuid
        constraint comment__reply__fk
            references PostComments on delete set null
);


drop table if exists PostLikes;
create table PostLikes
(
    PostId uuid
        constraint like__post__fk
            references Posts on delete cascade,
    UserId uuid
        constraint like__user__fk
            references Users on delete cascade,
    constraint liked__post__pk
        primary key (PostId, UserId)
);


drop table if exists Tags;
create table Tags
(
    Id   uuid primary key,
    Name varchar(255) not null
);

drop table if exists PostTags;
create table PostTags
(
    PostId uuid
        constraint post_tag__post__fk
            references Posts on delete cascade,
    TagId  uuid
        constraint post_tag__tag__fk
            references Tags on delete cascade,
    constraint post_tag__pk
        primary key (PostId, TagId)
);
