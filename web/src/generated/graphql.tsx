import gql from 'graphql-tag';
import * as Urql from 'urql';
export type Maybe<T> = T | null;
export type InputMaybe<T> = Maybe<T>;
export type Exact<T extends { [key: string]: unknown }> = { [K in keyof T]: T[K] };
export type MakeOptional<T, K extends keyof T> = Omit<T, K> & { [SubKey in K]?: Maybe<T[SubKey]> };
export type MakeMaybe<T, K extends keyof T> = Omit<T, K> & { [SubKey in K]: Maybe<T[SubKey]> };
export type Omit<T, K extends keyof T> = Pick<T, Exclude<keyof T, K>>;
/** All built-in and custom scalars, mapped to their actual values */
export type Scalars = {
  ID: string;
  String: string;
  Boolean: boolean;
  Int: number;
  Float: number;
  DateTime: any;
};

export type ConstIncome = {
  __typename?: 'ConstIncome';
  amount: Scalars['Float'];
  currency: Scalars['String'];
  id: Scalars['String'];
  name: Scalars['String'];
};

export type FieldError = {
  __typename?: 'FieldError';
  message: Scalars['String'];
  name: Scalars['String'];
};

export type Mutation = {
  __typename?: 'Mutation';
  login: UserPayload;
  register: UserPayload;
};


export type MutationLoginArgs = {
  input: UserInput;
};


export type MutationRegisterArgs = {
  input: UserInput;
};

export type Query = {
  __typename?: 'Query';
  constIncomes: Array<ConstIncome>;
  tempIncomes: Array<TempIncome>;
};

export type TempIncome = {
  __typename?: 'TempIncome';
  amount: Scalars['Float'];
  currency: Scalars['String'];
  dateTime: Scalars['DateTime'];
  id: Scalars['String'];
  name: Scalars['String'];
};

export type UserInput = {
  password: Scalars['String'];
  usernameOrEmail: Scalars['String'];
};

export type UserPayload = {
  __typename?: 'UserPayload';
  fieldErrors?: Maybe<Array<FieldError>>;
  usernameOrEmail: Scalars['String'];
};

export type RegisterMutationVariables = Exact<{
  input: UserInput;
}>;


export type RegisterMutation = { __typename?: 'Mutation', register: { __typename?: 'UserPayload', usernameOrEmail: string, fieldErrors?: Array<{ __typename?: 'FieldError', name: string, message: string }> | null | undefined } };

export type TempIncomesQueryVariables = Exact<{ [key: string]: never; }>;


export type TempIncomesQuery = { __typename?: 'Query', tempIncomes: Array<{ __typename?: 'TempIncome', name: string, amount: number }> };


export const RegisterDocument = gql`
    mutation Register($input: UserInput!) {
  register(input: $input) {
    usernameOrEmail
    fieldErrors {
      name
      message
    }
  }
}
    `;

export function useRegisterMutation() {
  return Urql.useMutation<RegisterMutation, RegisterMutationVariables>(RegisterDocument);
};
export const TempIncomesDocument = gql`
    query TempIncomes {
  tempIncomes {
    name
    amount
  }
}
    `;

export function useTempIncomesQuery(options: Omit<Urql.UseQueryArgs<TempIncomesQueryVariables>, 'query'> = {}) {
  return Urql.useQuery<TempIncomesQuery>({ query: TempIncomesDocument, ...options });
};