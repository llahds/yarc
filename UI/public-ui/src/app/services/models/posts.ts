import { KeyValueModel, PostedBy } from "./common";

export interface Post extends EditPost {
    id: number;
    forum: KeyValueModel;
    ups: number;
    downs: number;
    postedBy: PostedBy;
    createdOn: Date;
    canReport: boolean;
    vote: number;
    commentCount: number;
}

export interface EditPost {
    title: string;
    text: string;
}

export interface Comment {
    id: number;
    createdOn: Date;
    text: string;
    postedBy: PostedBy
    replyCount: number;
    ups: number;
    downs: number;
    vote: number;
    isDeleted: boolean;
    canReport: boolean;
    canEdit: boolean;
}