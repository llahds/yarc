import { Comment, Post } from "./posts";

export const SPAM_ID = 13;

export interface ReportingReason {
    id: number;
    label: string;
}

export interface QueueListWorkItem {
    post: Post;
    comment: Comment;
    reasons: string[];
}