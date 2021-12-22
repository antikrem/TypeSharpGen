// This is an auto generated test
import { AliasedDefinedTestsSubModel } from "./types2.d.ts";
import { DependentType } from "./MoreSub/DependentType.d.ts";

export interface TestModel {
    Name: string;
    Name2: string[];
    Child: TestSubModel;
    Child2: AliasedDefinedTestsSubModel;
    Dependent: DependentType;

    Something(subModel: AliasedDefinedTestsSubModel): DependentType[];
}

export interface TestSubModel {
    Value: number;
}
