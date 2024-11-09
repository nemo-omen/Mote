export type Note = {
    id: string;
    created: Date;
    createdBy: string;
    modified?: Date;
    modifiedBy?: string;
    title: string;
    content?: string;
    isArchived: boolean;
    isTask: boolean;
    isCompleted: boolean;
    dueDate?: Date;
    parentId?: string;
    parent?: string;
    children?: Note[];
    isRoot: boolean;
} | null;